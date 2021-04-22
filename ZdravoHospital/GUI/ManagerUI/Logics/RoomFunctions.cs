using System;
using System.Collections.Generic;
using System.Text;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public static class RoomFunctions
    {
        public static Room FindRoomByType(RoomType rt)
        {
            foreach (Room r in Model.Resources.rooms.Values)
            {
                if (r.Available == true && r.RoomType == rt)
                    return r;
            }

            return null;
        }

        public static Room FindRoomByType(RoomType rt, Room room)
        {
            foreach (Room r in Model.Resources.rooms.Values)
            {
                if (r.Available == true && r.RoomType == rt && r.Id != room.Id)
                    return r;
            }

            return null;
        }

        public static Room FindRoomByPrio()
        {
            Room someRoom = FindRoomByType(RoomType.STORAGE_ROOM);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.BREAK_ROOM);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.APPOINTMENT_ROOM);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.OPERATING_ROOM);

            return someRoom;
        }

        public static Room FindRoomByPrio(Room notThisRoom)
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

        public static bool DeleteRoom(Room room)
        {
            /* First handle its inventory */
            List<RoomInventory> roomsInventory = RoomInventoryFunctions.FindAllInventoryInRoom(room.Id);

            if(roomsInventory.Count != 0)
            {
                Room transportRoom = FindRoomByPrio(room);

                if (transportRoom == null)
                {
                    /* There are no rooms where this inventory would be transported */
                    return false;
                }

                /* Can be refactored when room transfering is added */

                List<RoomInventory> transportsRoomInventory = RoomInventoryFunctions.FindAllInventoryInRoom(transportRoom.Id);

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
            }

            /* Delete from dataBase and visual */
            ManagerWindow.Rooms.Remove(Model.Resources.rooms[room.Id]);
            Model.Resources.rooms.Remove(room.Id);

            Model.Resources.SerializeRoomInventory();
            Model.Resources.SerializeRooms();

            return true;
        }
    }
}
