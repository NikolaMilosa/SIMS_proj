using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomFunctions
    {
        private static Mutex _roomMutex;

        public static Mutex GetRoomMutex()
        {
            if (_roomMutex == null)
                _roomMutex = new Mutex();

            return _roomMutex;
        }

        public RoomFunctions() { }

        public Room FindRoomByType(RoomType rt, Room room)
        {
            if (room != null)
            {
                foreach (var r in Model.Resources.rooms.Values)
                {
                    if (r.Available == true && r.RoomType == rt && r.Id != room.Id)
                        return r;
                }
            }
            else
            {
                foreach (var r in Model.Resources.rooms.Values)
                {
                    if (r.Available == true && r.RoomType == rt)
                        return r;
                }
            }
            

            return null;
        }

        public Room FindRoomByPrio(Room notThisRoom)
        {
            var someRoom = FindRoomByType(RoomType.STORAGE_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.BED_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.APPOINTMENT_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.OPERATING_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.EMERGENCY_ROOM, notThisRoom);

            return someRoom;
        }

        public bool DeleteRoom(Room room)
        {
            GetRoomMutex().WaitOne();

            /* First handle its inventory */
            var roomInventoryService = new RoomInventoryFunctions();
            var roomsInventory = roomInventoryService.FindAllInventoryInRoom(room.Id);

            if(roomsInventory.Count != 0)
            {
                var transportRoom = FindRoomByPrio(room);

                if (transportRoom == null)
                {
                    /* There are no rooms where this inventory would be transported */
                    return false;
                }

                /* Can be refactored when room transferring is added, this transfers from a room being deleted 
                 * other suitable room */

                roomInventoryService.TransportRoomInventory(room, transportRoom);
            }
            
            /* Delete from dataBase and visual */
            //ManagerWindow.Rooms.Remove(Model.Resources.rooms[room.Id]);
            Model.Resources.rooms.Remove(room.Id);
            
            Model.Resources.SaveRooms();

            GetRoomMutex().ReleaseMutex();

            return true;
        }

        public void AddRoom(Room room)
        {
            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            GetRoomMutex().WaitOne();

            Model.Resources.rooms[room.Id] = room;
            //ManagerWindow.Rooms.Add(Model.Resources.rooms[room.Id]);
            Model.Resources.SaveRooms();

            GetRoomMutex().ReleaseMutex();
        }

        public void EditRoom(Room room)
        {
            GetRoomMutex().WaitOne();
            //var index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[room.Id]);
            //ManagerWindow.Rooms.Remove(Model.Resources.rooms[room.Id]);

            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            Model.Resources.rooms[room.Id] = room;
            Model.Resources.SaveRooms();

            //ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[room.Id]);
            GetRoomMutex().ReleaseMutex();
        }

        public void ChangeRoomAvailability(int roomId, bool newValue)
        {
            GetRoomMutex().WaitOne();
            /* Handle visuals */
            //var index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[roomId]);
            /*
            Application.Current.Dispatcher.BeginInvoke(new Func<bool>(
                () => ManagerWindow.Rooms.Remove(Model.Resources.rooms[roomId])));
            */
            Model.Resources.rooms[roomId].Available = newValue;
            Model.Resources.SaveRooms();
            /*
            Application.Current.Dispatcher.BeginInvoke(new Action(delegate ()
                { ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[roomId]); }));
            */
            /* If a room is show in manage inventory it should update */
            /*
            if (ManagerWindow.dialog != null)
            {
                if (ManagerWindow.dialog.GetType().Name.Equals(nameof(InventoryManagementWindow)))
                {
                    InventoryManagementWindow activeWindow = (InventoryManagementWindow)ManagerWindow.dialog;
                    

                    activeWindow.FirstRoom = activeWindow.FirstRoom;
                    activeWindow.SecondRoom = activeWindow.SecondRoom;
                }
            }
            */
            GetRoomMutex().ReleaseMutex();
        }
    }
}
