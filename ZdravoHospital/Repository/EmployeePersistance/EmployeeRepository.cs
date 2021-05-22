using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.EmployeePersistance
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static string _path = @"..\..\..\Resources\employees.json";
        private static Mutex _mutex;

        public EmployeeRepository()
        {
        }
        
        public void Create(Employee newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().WaitOne();
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.Username.Equals(id));
            GetMutex().ReleaseMutex();
        }

        public Employee GetById(string id)
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

        public List<Employee> GetValues()
        {
            GetMutex().WaitOne();

            var values = JsonConvert.DeserializeObject<List<Employee>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Employee>();
            }

            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<Employee> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(Employee newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}