using System;

namespace Model.Repository
{
    public class EmployeeRepository : Repository<string, Employee>
    {
        private static string path = @"..\..\..\Resources\employees.json";

        public EmployeeRepository() : base(path)
        {
        }

        public override Employee GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Employee newValue)
        {
            throw new NotImplementedException();
        }
    }
}