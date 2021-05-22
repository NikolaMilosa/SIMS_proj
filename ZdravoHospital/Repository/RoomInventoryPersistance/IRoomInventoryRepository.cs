using Model;
using System;

namespace Repository.RoomInventoryPersistance
{
   public interface IRoomInventoryRepository : IRepository<int, RoomInventory>
   {
       RoomInventory FindByBothIds(int roomId, string inventoryId);

       void DeleteByEquality(RoomInventory roomInventory);

       void DeleteByInventoryId(string inventoryId);
   }
}