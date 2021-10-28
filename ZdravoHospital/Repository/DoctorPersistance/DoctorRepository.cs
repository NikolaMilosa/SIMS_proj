using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Repository.DoctorPersistance
{
    public class DoctorRepository : IDoctorRepository
    {
        private static string _path = @"..\..\..\Resources\doctors.json";

        public void Create(Doctor newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Username.Equals(id));
            Save(values);
        }

        public Doctor GetById(string id)
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

        public List<Doctor> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Doctor>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Doctor>();
            }

            return values;
        }

        public void Save(List<Doctor> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Doctor newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }
    }
}