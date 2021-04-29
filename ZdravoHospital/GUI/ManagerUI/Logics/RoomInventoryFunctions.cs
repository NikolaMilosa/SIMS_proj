using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomInventoryFunctions
    {
        private static Mutex roomInventoryMutex;

        public static Mutex GetRoomInventoryMutex()
        {
            if (roomInventoryMutex == null)
                roomInventoryMutex = new Mutex();

            return roomInventoryMutex;
        }

        public RoomInventoryFunctions() { }

        public RoomInventory FindRoomInventoryByRoomAndInventory(int roomId, string inventoryId)
        {
            foreach (RoomInventory ri in Model.Resources.roomInventory)
            {
                if (ri.RoomId == roomId && ri.InventoryId.Equals(inventoryId))
                    return ri;
            }

            return null;
        }

        public List<RoomInventory> FindAllRoomsWithInventory(string inventoryId)
        {
            List<RoomInventory> ret = new List<RoomInventory>();

            foreach (RoomInventory ri in Model.Resources.roomInventory)
                if (ri.InventoryId.Equals(inventoryId))
                    ret.Add(ri);

            return ret;
        }

        public List<RoomInventory> FindAllInventoryInRoom(int roomId)
        {
            List<RoomInventory> ret = new List<RoomInventory>();

            foreach (RoomInventory ri in Model.Resources.roomInventory)
                if (ri.RoomId == roomId)
                    ret.Add(ri);

            return ret;
        }

        public void DeleteByInventoryId(string iid)
        {
            GetRoomInventoryMutex().WaitOne();
            Model.Resources.roomInventory.RemoveAll(ri => ri.InventoryId.Equals(iid));

            Model.Resources.SerializeRoomInventory();
            GetRoomInventoryMutex().ReleaseMutex();
        }

        public void DeleteByReference(RoomInventory ri)
        {
            GetRoomInventoryMutex().WaitOne();
            Model.Resources.roomInventory.Remove(ri);
            GetRoomInventoryMutex().ReleaseMutex();
        }

        public void AddNewReference(RoomInventory ri)
        {
            GetRoomInventoryMutex().WaitOne();
            Model.Resources.roomInventory.Add(ri);
            GetRoomInventoryMutex().ReleaseMutex();
        }
    }
}
