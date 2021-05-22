using Model;
using System;

namespace Repository.DoctorPersistance
{
   public interface IDoctorRepository : IRepository<string, Doctor>
   {
   }
}