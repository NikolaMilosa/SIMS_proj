using System;
using Model;

namespace Repository.InventoryRepository
{
   public interface IInventoryRepository : IRepository<string,Inventory>
   {
   }
}