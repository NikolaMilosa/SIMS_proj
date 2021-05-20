using System;
using System.Threading;

namespace Model.Repository
{
    public class EmployeeRepository : Repository<string, Employee>
    {
        private static string path = @"..\..\..\Resources\employees.json";

        public EmployeeRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override Employee GetById(string id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Username.Equals(id))
                {
                    return val;
                }
            }

            return null;
        }

        public override void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Username.Equals(id));
            Save(values);
        }

        public override void Update(Employee newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }
    }
}