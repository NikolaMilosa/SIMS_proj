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

        public InventoryRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
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
            var values = base.GetValues();
            GetMutex().WaitOne();
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
            var values = base.GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override void Update(Inventory newValue)
        {
            var values = base.GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}