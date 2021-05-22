using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.MedicineRecensionPersistance
{
    public class MedicineRecensionRepository : IMedicineRecensionRepository
    {
        private static string _path = @"..\..\..\Resources\medicineRecensions.json";
        private static Mutex _mutex;

        public MedicineRecensionRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }


        public void Create(MedicineRecension newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.MedicineName.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public MedicineRecension GetById(string id)
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

        public List<MedicineRecension> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<MedicineRecension>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<MedicineRecension>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<MedicineRecension> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(MedicineRecension newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}