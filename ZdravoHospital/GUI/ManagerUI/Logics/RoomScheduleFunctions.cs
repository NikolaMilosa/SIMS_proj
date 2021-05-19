using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomScheduleFunctions
    {
        private RoomRepository _roomRepository;

        private static Mutex _roomScheduleMutex;

        public static Mutex GetRoomScheduleMutex()
        {
            if (_roomScheduleMutex == null)
                _roomScheduleMutex = new Mutex();

            return _roomScheduleMutex;
        }

        public RoomScheduleFunctions()
        {
            _roomRepository = new RoomRepository();
        }

        public void RunOrExecute()
        {
            if (Model.Resources.roomSchedule.Count != 0)
            {
                List<RoomSchedule> loaded = new List<RoomSchedule>(Model.Resources.roomSchedule);
                foreach(RoomSchedule rs in loaded)
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
            var t = new Task(roomSchedule.WaitStartRenovation);
            t.Start();
        }

        public void ScheduleRenovationEnd(RoomSchedule roomSchedule)
        {
            if (!CheckIfStillValid(roomSchedule))
                return;

            var room = _roomRepository.GetById(roomSchedule.RoomId);

            if (room.Available)
            {
                var roomFunctions = new RoomFunctions();
                roomFunctions.ChangeRoomAvailability(roomSchedule.RoomId, false);
            }

            Task t = new Task(roomSchedule.WaitEndRenovation);
            t.Start();
        }

        public void FinishRenovation(RoomSchedule roomSchedule)
        {
            if (!CheckIfStillValid(roomSchedule))
                return;

            var room = _roomRepository.GetById(roomSchedule.RoomId);

            if (!room.Available && !IsInsideRenovation(roomSchedule))
            {
                var roomFunctions = new RoomFunctions();
                roomFunctions.ChangeRoomAvailability(roomSchedule.RoomId, true);
            }

            GetRoomScheduleMutex().WaitOne();

            if (Model.Resources.roomSchedule.RemoveAll(r => r.StartTime == roomSchedule.StartTime && 
                                                            r.EndTime == roomSchedule.EndTime &&
                                                            r.RoomId == roomSchedule.RoomId &&
                                                            r.ScheduleType == roomSchedule.ScheduleType) > 0)
            {
                Model.Resources.SaveRoomSchedule();
            }

            GetRoomScheduleMutex().ReleaseMutex();
        }

        public bool CheckIfStillValid(RoomSchedule roomSchedule)
        {
            if (!Model.Resources.roomSchedule.Contains(roomSchedule))
            {
                /* if this reference was not found in the list make it not valid */
                return false;
            }

            
            if (_roomRepository.GetById(roomSchedule.RoomId) == null)
            {
                GetRoomScheduleMutex().WaitOne();

                /* If the target room for this room schedule doesn't exist delete all other room schedules for this room.*/
                Model.Resources.roomSchedule.RemoveAll(rs => rs.RoomId == roomSchedule.RoomId);
                Model.Resources.SaveRoomSchedule();

                GetRoomScheduleMutex().ReleaseMutex();
                return false;
            }

            return true;
        }

        public bool IsInsideRenovation(RoomSchedule roomSchedule)
        {
            if (roomSchedule.ScheduleType != ReservationType.TRANSFER)
                return false;

            foreach (var rs in Model.Resources.roomSchedule)
            {
                if (rs.RoomId == roomSchedule.RoomId && rs.StartTime == roomSchedule.StartTime && rs.EndTime == roomSchedule.EndTime && rs.ScheduleType == roomSchedule.ScheduleType)
                    continue;
                if (roomSchedule.RoomId == rs.RoomId && rs.StartTime <= roomSchedule.EndTime && roomSchedule.EndTime <= rs.EndTime)
                    return true;
            }

            return false;
        }

        public void CreateAndScheduleRenovationStart(RoomSchedule roomSchedule)
        {
            GetRoomScheduleMutex().WaitOne();

            if (!CheckIfExists(roomSchedule))
            {
                Model.Resources.roomSchedule.Add(roomSchedule);
                Model.Resources.SaveRoomSchedule();
                ScheduleRenovationStart(roomSchedule);
            }

            GetRoomScheduleMutex().ReleaseMutex();
        }

        public bool CheckIfExists(RoomSchedule roomSchedule)
        {
            /* If two transfer requests are created for the same room as receiver room it is possible that those two rooms will have the
             exact same room schedule. Therefore, do not include its duplicate.*/
            var exists = false;

            Model.Resources.roomSchedule.ForEach(rs =>
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

        public ObservableCollection<ReservationDTO> GetReservationsForRoom(Room room, DateTime day)
        {
            var reservations = new ObservableCollection<ReservationDTO>();

            var end = day.AddDays(1);
            Model.Resources.periods.ForEach(p =>
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

            Model.Resources.roomSchedule.ForEach(r =>
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
