using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class MergeRoomService
    {
        private IRoomRepository _roomRepository;
        private IRoomInventoryRepository _roomInventoryRepository;

        public MergeRoomService(InjectorDTO injector)
        {
            _roomRepository = injector.RoomRepository;
            _roomInventoryRepository = injector.RoomInventoryRepository;
        }

        public void MergeRooms(Room remainingRoom, Room mergedRoom)
        {
            var remainingRoomInventory = _roomInventoryRepository.FindAllInventoryInRoom(remainingRoom.Id);
            var mergedRoomInventory = _roomInventoryRepository.FindAllInventoryInRoom(mergedRoom.Id);

            foreach (var roomInventory in mergedRoomInventory)
            {
                var remainingInstance = remainingRoomInventory.Find(val => val.RoomId == remainingRoom.Id
                                                                           && val.InventoryId.Equals(roomInventory.InventoryId));
                if (remainingInstance != null)
                {
                    _roomInventoryRepository.SetNewQuantity(remainingInstance, remainingInstance.Quantity + roomInventory.Quantity);
                }
                else
                {
                    _roomInventoryRepository.Create(new RoomInventory(roomInventory.InventoryId, remainingRoom.Id, roomInventory.Quantity));
                }
            }

            _roomInventoryRepository.DeleteByRoomId(mergedRoom.Id);
            _roomRepository.DeleteById(mergedRoom.Id);
        }

    }
}
