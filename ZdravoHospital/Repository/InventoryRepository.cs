using System;
using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading;

namespace Model.Repository
{
    public class InventoryRepository : Repository<string, Inventory>
    {
        private static string path = @"..\..\..\Resources\inventory.json";
        private static Mutex mutex;

        private Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public InventoryRepository() : base(path)
        {
        }

        public override List<Inventory> GetValues()
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            GetMutex().ReleaseMutex();
            return values;
        }

        public override Inventory GetById(string id)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            foreach (var inv in values)
            {
                if (inv.Id.Equals(id))
                {
                    GetMutex().ReleaseMutex();
                    return inv;
                }
            }

            GetMutex().ReleaseMutex();
            return null;
        }

        public override void DeleteById(string id)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Inventory newValue)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Create(Inventory newValue)
        {
            GetMutex().WaitOne();
            base.Create(newValue);
            GetMutex().ReleaseMutex();
        }
    }
}