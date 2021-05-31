using Model;
using System;
using System.Collections.Generic;

namespace Repository.RoomInventoryPersistance
{
   public interface IRoomInventoryRepository : IRepository<int, RoomInventory>
   {
       RoomInventory FindByBothIds(int roomId, string inventoryId);

       void DeleteByEquality(RoomInventory roomInventory);

       void DeleteByInventoryId(string inventoryId);

       List<RoomInventory> FindAllInventoryInRoom(int roomId);

       void SetNewQuantity(RoomInventory roomInventory, int newQuantity);

       void DeleteByRoomId(int id);
   }
}