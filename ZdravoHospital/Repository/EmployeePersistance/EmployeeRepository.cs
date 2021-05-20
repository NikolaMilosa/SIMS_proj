using System.Collections.Generic;
using System.IO;
using Model;
using Model.Repository;
using Newtonsoft.Json;

namespace ZdravoHospital.Repository.EmployeePersistance
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private static string path = @"..\..\..\Resources\employees.json";

        public void Save(List<Employee> values)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public Employee GetById(string key)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Username.Equals(key))
                {
                    return val;
                }
            }

            return null;
        }

        public void DeleteById(string key)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Username.Equals(key));
            Save(values);
        }

        public void Update(Employee newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }

        public List<Employee> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Employee>>(path);
            return values;
        }

        public void Create(Employee newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }
    }
}