using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
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
                someRoom = FindRoomByType(RoomType.BREAK_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.APPOINTMENT_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.OPERATING_ROOM, notThisRoom);

            return someRoom;
        }

        public bool DeleteRoom(Room room)
        {
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

                var transportsRoomInventory = roomInventoryService.FindAllInventoryInRoom(transportRoom.Id);

                GetRoomMutex().WaitOne();

                foreach (var ri in roomsInventory)
                {
                    var handled = false;
                    foreach(var tri in transportsRoomInventory)
                    {
                        if (tri.InventoryId.Equals(ri.InventoryId))
                        {
                            tri.Quantity += ri.Quantity;
                            handled = true;
                            break;
                        }
                    }

                    if (!handled)
                    {
                        Model.Resources.roomInventory.Add(new RoomInventory(ri.InventoryId, transportRoom.Id, ri.Quantity));
                    }

                    Model.Resources.roomInventory.Remove(ri);
                }

                GetRoomMutex().ReleaseMutex();
            }

            GetRoomMutex().WaitOne();

            /* Delete from dataBase and visual */
            ManagerWindow.Rooms.Remove(Model.Resources.rooms[room.Id]);
            Model.Resources.rooms.Remove(room.Id);

            Model.Resources.SerializeRoomInventory();
            Model.Resources.SerializeRooms();

            GetRoomMutex().ReleaseMutex();

            return true;
        }

        public void AddRoom(Room room)
        {
            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            GetRoomMutex().WaitOne();

            Model.Resources.rooms[room.Id] = room;
            ManagerWindow.Rooms.Add(Model.Resources.rooms[room.Id]);
            Model.Resources.SerializeRooms();

            GetRoomMutex().ReleaseMutex();
        }

        public void EditRoom(Room room)
        {
            GetRoomMutex().WaitOne();
            var index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[room.Id]);
            ManagerWindow.Rooms.Remove(Model.Resources.rooms[room.Id]);

            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            Model.Resources.rooms[room.Id] = room;
            Model.Resources.SerializeRooms();

            ManagerWindow.Rooms.Insert(index, Model.Resources.rooms[room.Id]);
            GetRoomMutex().ReleaseMutex();
        }        
    }
}
