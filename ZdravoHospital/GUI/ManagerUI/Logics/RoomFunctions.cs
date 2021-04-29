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
        private static Mutex roomMutex;

        public static Mutex GetRoomMutex()
        {
            if (roomMutex == null)
                roomMutex = new Mutex();

            return roomMutex;
        }

        public RoomFunctions() { }

        public Room FindRoomByType(RoomType rt, Room room)
        {
            if (room != null)
            {
                foreach (Room r in Model.Resources.rooms.Values)
                {
                    if (r.Available == true && r.RoomType == rt && r.Id != room.Id)
                        return r;
                }
            }
            else
            {
                foreach (Room r in Model.Resources.rooms.Values)
                {
                    if (r.Available == true && r.RoomType == rt)
                        return r;
                }
            }
            

            return null;
        }

        public Room FindRoomByPrio(Room notThisRoom)
        {
            Room someRoom = FindRoomByType(RoomType.STORAGE_ROOM, notThisRoom);

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
            RoomInventoryFunctions roomInventoryService = new RoomInventoryFunctions();
            List<RoomInventory> roomsInventory = roomInventoryService.FindAllInventoryInRoom(room.Id);

            if(roomsInventory.Count != 0)
            {
                Room transportRoom = FindRoomByPrio(room);

                if (transportRoom == null)
                {
                    /* There are no rooms where this inventory would be transported */
                    return false;
                }

                /* Can be refactored when room transfering is added, this transfers from a room being deleted 
                 * other suitable room */

                List<RoomInventory> transportsRoomInventory = roomInventoryService.FindAllInventoryInRoom(transportRoom.Id);

                GetRoomMutex().WaitOne();

                foreach (RoomInventory ri in roomsInventory)
                {
                    bool handeled = false;
                    foreach(RoomInventory tri in transportsRoomInventory)
                    {
                        if (tri.InventoryId.Equals(ri.InventoryId))
                        {
                            tri.Quantity += ri.Quantity;
                            handeled = true;
                            break;
                        }
                    }

                    if (!handeled)
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
            int index = ManagerWindow.Rooms.IndexOf(Model.Resources.rooms[room.Id]);
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
