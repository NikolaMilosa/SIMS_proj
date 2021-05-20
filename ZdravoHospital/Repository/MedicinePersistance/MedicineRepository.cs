using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class MedicineRepository : IMedicineRepository
    {
        private static string path = @"..\..\..\Resources\medicines.json";

        private static Mutex mutex;

        public void Save(List<Medicine> values)
        {
            throw new NotImplementedException();
        }

        public Medicine GetById(string key)
        {
            throw new NotImplementedException();
        }

        public void DeleteById(string key)
        {
            throw new NotImplementedException();
        }

        public void Update(Medicine newValue)
        {
            throw new NotImplementedException();
        }

        public List<Medicine> GetValues()
        {
            throw new NotImplementedException();
        }

        public void Create(Medicine newValue)
        {
            throw new NotImplementedException();
        }

        Mutex GetMutex()
        {
            throw new NotImplementedException();
        }
    }
}