using Model;
using System;

namespace Repository.EmployeePersistance
{
   public interface IEmployeeRepository : IRepository<string, Employee>
   {
   }
}