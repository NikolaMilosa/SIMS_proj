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

        private Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public MedicineRepository() : base(path)
        {
        }

        public override List<Medicine> GetValues()
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            GetMutex().ReleaseMutex();
            return values;
        }

        public override void Create(Medicine newValue)
        {
            GetMutex().WaitOne();
            base.Create(newValue);
            GetMutex().ReleaseMutex();
        }

        public override Medicine GetById(string id)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
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
            GetMutex().WaitOne();
            var values = base.GetValues();
            values.RemoveAll(val => val.MedicineName.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Medicine newValue)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values[values.FindIndex(val => val.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}