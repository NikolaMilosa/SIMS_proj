using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Input;

namespace Model.Repository
{
    public class MedicineRepository : Repository<string, Medicine>
    {
        private static string path = @"..\..\..\Resources\medicines.json";

        private static Mutex mutex;

        public MedicineRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override Medicine GetById(string id)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            foreach (var val in values)
            {
                if (val.MedicineName.Equals(id))
                {
                    GetMutex().ReleaseMutex();
                    return val;
                }
            }

            GetMutex().ReleaseMutex();
            return null;
        }

        public override void DeleteById(string id)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.MedicineName.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Medicine newValue)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}