using System;
using Model;

namespace Repository.InventoryPersistance
{
   public interface IInventoryRepository : IRepository<string,Inventory>
   {
        public Inventory GetByName(string name);
   }
}