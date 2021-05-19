using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomInventoryFunctions
    {
        private RoomInventoryRepository _roomInventoryRepository;

        private static Mutex mutex;

        private Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();
            return mutex;
        }

        public RoomInventoryFunctions()
        {
            _roomInventoryRepository = new RoomInventoryRepository();
        }

        public RoomInventory FindRoomInventoryByRoomAndInventory(int roomId, string inventoryId)
        {
            foreach (RoomInventory ri in _roomInventoryRepository.GetValues())
            {
                if (ri.RoomId == roomId && ri.InventoryId.Equals(inventoryId))
                    return ri;
            }

            return null;
        }

        public List<RoomInventory> FindAllInventoryInRoom(int roomId)
        {
            var ret = new List<RoomInventory>();

            foreach (var ri in _roomInventoryRepository.GetValues())
                if (ri.RoomId == roomId)
                    ret.Add(ri);

            return ret;
        }

        public void DeleteByInventoryId(string iid)
        {
            _roomInventoryRepository.DeleteByInventoryId(iid);
        }

        public void DeleteByReference(RoomInventory ri)
        {
            _roomInventoryRepository.DeleteByEquality(ri);
        }

        public void AddNewReference(RoomInventory ri)
        {
            _roomInventoryRepository.Create(ri);
        }

        public void SetNewQuantity(RoomInventory roomInventory, int newQuantity)
        {   
            roomInventory.Quantity = newQuantity;
            _roomInventoryRepository.Update(roomInventory);
        }

        public void TransportRoomInventory(Room firstRoom, Room secondRoom)
        {
            GetMutex().WaitOne();
            var firstRoomInventory = FindAllInventoryInRoom(firstRoom.Id);
            var secondRoomInventory = FindAllInventoryInRoom(secondRoom.Id);

            foreach (var roomInventory in firstRoomInventory)
            {
                var existenceInSecondRoom = secondRoomInventory.Find(ri => ri.InventoryId.Equals(roomInventory.InventoryId));
                if (existenceInSecondRoom == null)
                {
                    /* doesn't exist there */
                    var newRoomInventory = new RoomInventory(roomInventory.InventoryId, secondRoom.Id, roomInventory.Quantity);
                    AddNewReference(newRoomInventory);
                }
                else
                {
                    /* just edit its quantity */
                    SetNewQuantity(existenceInSecondRoom, roomInventory.Quantity + existenceInSecondRoom.Quantity);
                }

                /* delete the reference */
                DeleteByReference(roomInventory);
            }
            GetMutex().ReleaseMutex();
        }
    }
}
