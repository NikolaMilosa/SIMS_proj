using Model;
using System;

namespace Repository.PatientPersistance
{
   public interface IPatientRepository : IRepository<string, Patient>
   {
   }
}