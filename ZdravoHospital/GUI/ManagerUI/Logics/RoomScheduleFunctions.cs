using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Model;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomScheduleFunctions
    {
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
                /* Handle visuals */
                
                int index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[roomSchedule.RoomId]);
                Application.Current.Dispatcher.BeginInvoke(new Func<bool>(
                                    () => ManagerWindow.Rooms.Remove(Model.Resources.rooms[roomSchedule.RoomId])));

                Model.Resources.rooms[roomSchedule.RoomId].Available = false;
                Model.Resources.SerializeRooms();

                Application.Current.Dispatcher.BeginInvoke(new Action(delegate () 
                                    { ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[roomSchedule.RoomId]); }));
            }

            Task t = new Task(() => roomSchedule.WaitEndRenovation());
            t.Start();
        }

        public void FinishRenovation(RoomSchedule roomSchedule)
        {
            if (!Model.Resources.rooms[roomSchedule.RoomId].Available)
            {
                /* Handle visuals */
                int index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[roomSchedule.RoomId]);
                Application.Current.Dispatcher.BeginInvoke(new Func<bool>(
                                    () => ManagerWindow.Rooms.Remove(Model.Resources.rooms[roomSchedule.RoomId])));

                Model.Resources.rooms[roomSchedule.RoomId].Available = true;
                Model.Resources.SerializeRooms();

                Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                { ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[roomSchedule.RoomId]); }));
            }

            if (Model.Resources.roomSchedule.Remove(roomSchedule))
            {
                Model.Resources.SerializeRoomSchedule();
            }
        }

        public void CreateAndScheduleRenovationStart(RoomSchedule roomSchedule)
        {
            Model.Resources.roomSchedule.Add(roomSchedule);
            Model.Resources.SerializeRoomSchedule();
            ScheduleRenovationStart(roomSchedule);
        }
    }
}
