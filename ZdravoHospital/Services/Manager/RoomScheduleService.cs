using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using Repository.RoomSchedulePersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class RoomScheduleService
    {
        private InjectorDTO _injector;

        private static Mutex _mutex;

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        #region Repos

        private IRoomScheduleRepository _roomScheduleRepository;
        private IRoomRepository _roomRepository;
        private IPeriodRepository _periodRepository;

        #endregion

        #region Event things

        public delegate void RoomChangedEventHandler(object sender, EventArgs e);

        public event RoomChangedEventHandler RoomChanged;

        protected virtual void OnRoomChanged()
        {
            if (RoomChanged != null)
            {
                RoomChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        public RoomScheduleService(InjectorDTO injector)
        {
            RoomChanged += ManagerWindowViewModel.GetDashboard().OnRoomsChanged;

            _injector = injector;

            _roomRepository = injector.RoomRepository;
            _roomScheduleRepository = injector.RoomScheduleRepository;
            _periodRepository = injector.PeriodRepository;
        }

        public void RunOrExecute()
        {
            var values = _roomScheduleRepository.GetValues();
            if (values.Count != 0)
            {
                List<RoomSchedule> loaded = new List<RoomSchedule>(values);
                foreach (RoomSchedule rs in loaded)
                {
                    if (DateTime.Now < rs.StartTime)
                    {
                        /* the time to start renovation hasn't come yet */
                        ScheduleRenovationStart(rs);
                    }
                    else if (rs.StartTime <= DateTime.Now && DateTime.Now < rs.EndTime)
                    {
                        /* renovation in progress */
                        ScheduleRenovationEnd(rs);
                    }
                    else if (rs.EndTime < DateTime.Now)
                    {
                        /* renovation has ended */
                        FinishRenovation(rs);
                    }
                }
            }
        }

        public void ScheduleRenovationStart(RoomSchedule roomSchedule)
        {
            var t = new Task(() => roomSchedule.WaitStartRenovation(_injector));
            t.Start();
        }

        public void ScheduleRenovationEnd(RoomSchedule roomSchedule)
        {
            GetMutex().WaitOne();
            if (!CheckIfStillValid(roomSchedule))
            {
                GetMutex().ReleaseMutex();
                return;
            }

            var room = _roomRepository.GetById(roomSchedule.RoomId);

            if (room.Available)
            {
                ChangeRoomAvailability(roomSchedule.RoomId, false);
            }
            
            GetMutex().ReleaseMutex();
            Task t = new Task(() => roomSchedule.WaitEndRenovation(_injector));
            t.Start();
        }

        public void FinishRenovation(RoomSchedule roomSchedule)
        {
            GetMutex().WaitOne();
            if (!CheckIfStillValid(roomSchedule))
            {
                GetMutex().ReleaseMutex();
                return;
            }

            var room = _roomRepository.GetById(roomSchedule.RoomId);

            if (!room.Available && !IsInsideRenovation(roomSchedule))
            {
                ChangeRoomAvailability(roomSchedule.RoomId, true);
            }

            if (roomSchedule.WillBeMerged)
            {
                var mergeService = new MergeRoomService(_injector);
                mergeService.MergeRooms(_roomRepository.GetById(roomSchedule.MergingRoomId),
                    _roomRepository.GetById(roomSchedule.RoomId));
                OnRoomChanged();
            }

            GetMutex().ReleaseMutex();
            _roomScheduleRepository.DeleteByEquality(roomSchedule);
        }

        private void ChangeRoomAvailability(int roomId, bool newValue)
        {
            var room = _roomRepository.GetById(roomId);
            room.Available = newValue;

            _roomRepository.Update(room);

            OnRoomChanged();
        }

        private bool CheckIfStillValid(RoomSchedule roomSchedule)
        {
            if (!_roomScheduleRepository.ExistsInDataBase(roomSchedule))
            {
                /* if this reference was not found in the list make it not valid */
                return false;
            }


            if (_roomRepository.GetById(roomSchedule.RoomId) == null)
            {
                _roomScheduleRepository.DeleteByRoomId(roomSchedule.RoomId);
                return false;
            }

            return true;
        }

        private bool IsInsideRenovation(RoomSchedule roomSchedule)
        {
            if (roomSchedule.ScheduleType != ReservationType.TRANSFER)
                return false;

            foreach (var rs in _roomScheduleRepository.GetValues())
            {
                if (rs.RoomId == roomSchedule.RoomId && rs.StartTime == roomSchedule.StartTime 
                                                     && rs.EndTime == roomSchedule.EndTime && rs.ScheduleType == roomSchedule.ScheduleType)
                    continue;
                if (roomSchedule.RoomId == rs.RoomId && rs.StartTime <= roomSchedule.EndTime && roomSchedule.EndTime <= rs.EndTime)
                    return true;
            }

            return false;
        }

        public void CreateAndScheduleRenovationStart(RoomSchedule roomSchedule)
        {
            if (!CheckIfExists(roomSchedule))
            {
                _roomScheduleRepository.Create(roomSchedule);
                ScheduleRenovationStart(roomSchedule);
            }
        }

        private bool CheckIfExists(RoomSchedule roomSchedule)
        {
            /* If two transfer requests are created for the same room as receiver room it is possible that those two rooms will have the
             exact same room schedule. Therefore, do not include its duplicate.*/
            var exists = false;

            _roomScheduleRepository.GetValues().ForEach(rs =>
            {
                if (rs.RoomId == roomSchedule.RoomId && rs.StartTime == roomSchedule.StartTime &&
                    rs.EndTime == roomSchedule.EndTime)
                    exists = true;
            });

            return exists;
        }

        public ObservableCollection<RoomScheduleDTO> GetRoomSchedule(Room room)
        {
            var roomSchedule = new ObservableCollection<RoomScheduleDTO>();

            /* How many days ahead to show */
            var end = DateTime.Today.AddDays(14);

            for (var begin = DateTime.Today; begin <= end; begin = begin.AddDays(1))
            {
                var roomScheduleInstance = new RoomScheduleDTO(begin)
                {
                    Reservations = GetReservationsForRoom(room, begin)
                };
                roomSchedule.Add(roomScheduleInstance);
            }

            return roomSchedule;
        }

        private ObservableCollection<ReservationDTO> GetReservationsForRoom(Room room, DateTime day)
        {
            var reservations = new ObservableCollection<ReservationDTO>();

            var end = day.AddDays(1);
            _periodRepository.GetValues().ForEach(p =>
            {
                if (p.StartTime >= day && p.StartTime < end && p.RoomId == room.Id)
                {
                    var rt = ReservationType.RENOVATION;
                    if (p.PeriodType == PeriodType.APPOINTMENT)
                        rt = ReservationType.APPOINTMENT;
                    else if (p.PeriodType == PeriodType.OPERATION)
                        rt = ReservationType.OPERATION;

                    var reservationEnd = p.StartTime.AddMinutes(p.Duration);

                    var reservation = new ReservationDTO(rt, p.StartTime, reservationEnd);
                    reservations.Add(reservation);
                }
            });

            _roomScheduleRepository.GetValues().ForEach(r =>
            {
                if (r.RoomId == room.Id)
                {
                    if ((r.StartTime >= day && r.StartTime < end) || (day >= r.StartTime && end <= r.EndTime) || (r.EndTime >= day && r.EndTime < end))
                    {
                        /* Starts today */
                        var reservation = new ReservationDTO(r.ScheduleType, r.StartTime, r.EndTime);
                        reservations.Add(reservation);
                    }
                }
            });
            return reservations;
        }
    }
}
