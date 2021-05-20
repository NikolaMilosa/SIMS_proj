using System;
using System.Threading;
using System.Windows.Documents;

namespace Model.Repository
{
    public class MedicineRecensionRepository : Repository<string, MedicineRecension>
    {
        private static string path = @"..\..\..\Resources\medicineRecensions.json";
        private static Mutex mutex;

        public MedicineRecensionRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override MedicineRecension GetById(string id)
        {
            var values = GetValues();
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
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.MedicineName.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(MedicineRecension newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}