using Model;
using System;
using System.Threading;

namespace Repository.MedicinePersistance
{
   public interface IMedicineRepository : IRepository<string, Medicine>
   {
   }
}