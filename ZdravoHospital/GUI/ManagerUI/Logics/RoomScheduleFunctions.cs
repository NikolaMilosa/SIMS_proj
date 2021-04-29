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
        private static Mutex roomScheduleMutex;

        public static Mutex GetRoomScheduleMutex()
        {
            if (roomScheduleMutex == null)
                roomScheduleMutex = new Mutex();

            return roomScheduleMutex;
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
            Task t = new Task(() => roomSchedule.WaitStartRenovation());
            t.Start();
        }

        public void ScheduleRenovationEnd(RoomSchedule roomSchedule)
        {
            if (Model.Resources.rooms[roomSchedule.RoomId].Available)
            {
                GetRoomScheduleMutex().WaitOne();
                /* Handle visuals */
                
                int index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[roomSchedule.RoomId]);
                Application.Current.Dispatcher.BeginInvoke(new Func<bool>(
                                    () => ManagerWindow.Rooms.Remove(Model.Resources.rooms[roomSchedule.RoomId])));

                Model.Resources.rooms[roomSchedule.RoomId].Available = false;
                Model.Resources.SerializeRooms();

                Application.Current.Dispatcher.BeginInvoke(new Action(delegate () 
                                    { ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[roomSchedule.RoomId]); }));
                GetRoomScheduleMutex().ReleaseMutex();
            }

            Task t = new Task(() => roomSchedule.WaitEndRenovation());
            t.Start();
        }

        public void FinishRenovation(RoomSchedule roomSchedule)
        {
            if (!Model.Resources.rooms[roomSchedule.RoomId].Available && !IsInsideRenovation(roomSchedule))
            {
                GetRoomScheduleMutex().WaitOne();
                /* Handle visuals */
                int index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[roomSchedule.RoomId]);
                Application.Current.Dispatcher.BeginInvoke(new Func<bool>(
                                    () => ManagerWindow.Rooms.Remove(Model.Resources.rooms[roomSchedule.RoomId])));

                Model.Resources.rooms[roomSchedule.RoomId].Available = true;
                Model.Resources.SerializeRooms();

                Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                    { ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[roomSchedule.RoomId]); }));

                GetRoomScheduleMutex().ReleaseMutex();
            }

            if (Model.Resources.roomSchedule.Remove(roomSchedule))
            {
                GetRoomScheduleMutex().WaitOne();
                Model.Resources.SerializeRoomSchedule();
                GetRoomScheduleMutex().ReleaseMutex();
            }
        }

        public bool IsInsideRenovation(RoomSchedule roomSchedule)
        {
            if (roomSchedule.ScheduleType != ReservationType.TRANSFER)
                return false;

            foreach (RoomSchedule rs in Model.Resources.roomSchedule)
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
            ObservableCollection<RoomScheduleDTO> roomSchedule = new ObservableCollection<RoomScheduleDTO>();

            /* How many days ahead to show */
            DateTime end = DateTime.Today.AddDays(14);

            for (DateTime begin = DateTime.Today; begin <= end; begin = begin.AddDays(1))
            {
                RoomScheduleDTO roomScheduleInstance = new RoomScheduleDTO(begin);
                roomScheduleInstance.Reservations = GetReservationsForRoom(room, begin);
                roomSchedule.Add(roomScheduleInstance);
            }

            return roomSchedule;
        }

        public ObservableCollection<ReservationDTO> GetReservationsForRoom(Room room, DateTime day)
        {
            ObservableCollection<ReservationDTO> reservations = new ObservableCollection<ReservationDTO>();

            DateTime end = day.AddDays(1);
            Model.Resources.periods.ForEach(p =>
            {
                if (p.StartTime >= day && p.StartTime < end && p.RoomId == room.Id)
                {
                    ReservationType rt = ReservationType.RENOVATION;
                    if (p.PeriodType == PeriodType.APPOINTMENT)
                        rt = ReservationType.APPOINTMENT;
                    else if (p.PeriodType == PeriodType.OPERATION)
                        rt = ReservationType.OPERATION;

                    DateTime reservationEnd = p.StartTime.AddMinutes(p.Duration);

                    ReservationDTO reservation = new ReservationDTO(rt, p.StartTime, reservationEnd);
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
                        ReservationDTO reservation = new ReservationDTO(r.ScheduleType, r.StartTime, r.EndTime);
                        reservations.Add(reservation);
                    }
                }
            });
            return reservations;
        }
    }
}
