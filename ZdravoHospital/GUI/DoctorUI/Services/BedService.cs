using Model;
using Repository.InventoryPersistance;
using Repository.RoomInventoryPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class BedService
    {
        InventoryRepository _inventoryRepository;
        RoomInventoryRepository _roomInventoryRepository;

        public BedService()
        {
            _inventoryRepository = new InventoryRepository();
            _roomInventoryRepository = new RoomInventoryRepository();
        }

        public int GetRoomBedCount(int roomId)
        {
            int count = 0;
            string bedInventoryId = _inventoryRepository.GetByName("bed").Id;
            
            foreach (RoomInventory roomInventory in _roomInventoryRepository.GetValues())
                if (roomInventory.RoomId == roomId && roomInventory.InventoryId == bedInventoryId)
                    count++;

            return count;
        }
    }
}
