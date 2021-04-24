﻿using System;
using System.Collections.Generic;
using System.Text;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public static class RoomInventoryFunctions
    {
        public static RoomInventory FindRoomInventoryByRoomAndInventory(int roomId, string inventoryId)
        {
            foreach (RoomInventory ri in Model.Resources.roomInventory)
            {
                if (ri.RoomId == roomId && ri.InventoryId.Equals(inventoryId))
                    return ri;
            }

            return null;
        }

        public static List<RoomInventory> FindAllRoomsWithInventory(string inventoryId)
        {
            List<RoomInventory> ret = new List<RoomInventory>();

            foreach (RoomInventory ri in Model.Resources.roomInventory)
                if (ri.InventoryId.Equals(inventoryId))
                    ret.Add(ri);

            return ret;
        }

        public static List<RoomInventory> FindAllInventoryInRoom(int roomId)
        {
            List<RoomInventory> ret = new List<RoomInventory>();

            foreach (RoomInventory ri in Model.Resources.roomInventory)
                if (ri.RoomId == roomId)
                    ret.Add(ri);

            return ret;
        }

        public static void DeleteByInventoryId(string iid)
        {
            Model.Resources.roomInventory.RemoveAll(ri => ri.InventoryId.Equals(iid));

            Model.Resources.SerializeRoomInventory();
        }

        public static void DeleteByReference(RoomInventory ri)
        {
            Model.Resources.roomInventory.Remove(ri);
        }

        public static void AddNewReference(RoomInventory ri)
        {
            Model.Resources.roomInventory.Add(ri);
        }
    }
}