using Model;
using System;

namespace Repository.MedicinePersistance
{
   public interface IMedicineRepository : IRepository<string, Medicine>
   {
   }
}