using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.MedicinePersistance
{
    public class MedicineRepository : IMedicineRepository
    {
        private static string _path = @"..\..\..\Resources\medicines.json";
        private static Mutex _mutex;

        public MedicineRepository()
        {
        }

        public void Create(Medicine newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
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
            values.RemoveAll(val => val.MedicineName.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public Medicine GetById(string id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.MedicineName.Equals(id))
                {
                    return val;
                }
            }

            return null;
        }

        public List<Medicine> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<Medicine>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Medicine>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<Medicine> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(Medicine newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}