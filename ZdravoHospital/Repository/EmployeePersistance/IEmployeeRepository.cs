using Model;
using System;
using System.Threading;

namespace Repository.EmployeePersistance
{
   public interface IEmployeeRepository : IRepository<string, Employee>
   {
   }
}