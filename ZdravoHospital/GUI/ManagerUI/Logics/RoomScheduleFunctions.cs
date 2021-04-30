using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using Model;

using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomScheduleFunctions
    {
        private static Mutex _roomScheduleMutex;

        public static Mutex GetRoomScheduleMutex()
        {
            if (_roomScheduleMutex == null)
                _roomScheduleMutex = new Mutex();

            return _roomScheduleMutex;
        }

        public RoomScheduleFunctions() { }

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
            if (!Model.Resources.rooms.ContainsKey(roomSchedule.RoomId))
            {
                GetRoomScheduleMutex().WaitOne();
                
                Model.Resources.roomSchedule.RemoveAll(rs => rs.RoomId == roomSchedule.RoomId);
                Model.Resources.SerializeRoomSchedule();

                GetRoomScheduleMutex().ReleaseMutex();
                return;
            }

            if (Model.Resources.rooms[roomSchedule.RoomId].Available)
            {
                var roomFunctions = new RoomFunctions();
                roomFunctions.ChangeRoomAvailability(roomSchedule.RoomId, false);
            }

            Task t = new Task(roomSchedule.WaitEndRenovation);
            t.Start();
        }

        public void FinishRenovation(RoomSchedule roomSchedule)
        {
            if (!Model.Resources.rooms.ContainsKey(roomSchedule.RoomId))
            {
                GetRoomScheduleMutex().WaitOne();

                Model.Resources.roomSchedule.RemoveAll(rs => rs.RoomId == roomSchedule.RoomId);
                Model.Resources.SerializeRoomSchedule();

                GetRoomScheduleMutex().ReleaseMutex();
                return;
            }

            if (!Model.Resources.rooms[roomSchedule.RoomId].Available && !IsInsideRenovation(roomSchedule))
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
                Model.Resources.SerializeRoomSchedule();
            }
            GetRoomScheduleMutex().ReleaseMutex();
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

            Model.Resources.roomSchedule.Add(roomSchedule);
            Model.Resources.SerializeRoomSchedule();
            ScheduleRenovationStart(roomSchedule);
            
            GetRoomScheduleMutex().ReleaseMutex();
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
