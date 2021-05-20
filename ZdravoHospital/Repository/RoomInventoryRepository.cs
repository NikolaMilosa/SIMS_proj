using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class RoomInventoryRepository : Repository<int, RoomInventory>
    {
        private static string path = @"..\..\..\Resources\roomInventory.json";
        private static Mutex mutex;

        private Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();
            return mutex;
        }

        public RoomInventoryRepository() : base(path)
        {
        }

        public override RoomInventory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(RoomInventory newValue)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values[
                values.FindIndex(val => val.RoomId == newValue.RoomId && newValue.InventoryId.Equals(val.InventoryId))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public override List<RoomInventory> GetValues()
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            GetMutex().ReleaseMutex();
            return values;
        }

        public override void Create(RoomInventory newValue)
        {
            GetMutex().WaitOne();
            base.Create(newValue);
            GetMutex().ReleaseMutex();
        }

        public RoomInventory FindByBothIds(int roomId, string inventoryId)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            foreach (var ri in values)
            {
                if (ri.RoomId == roomId && inventoryId.Equals(ri.InventoryId))
                {
                    GetMutex().ReleaseMutex();
                    return ri;
                }
            }

            GetMutex().ReleaseMutex();
            return null;
        }

        public void DeleteByEquality(RoomInventory roomInventory)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values.RemoveAll(
                ri => ri.RoomId == roomInventory.RoomId && ri.InventoryId.Equals(roomInventory.InventoryId));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteByInventoryId(string inventoryId)
        {
            GetMutex().WaitOne();
            var values = base.GetValues();
            values.RemoveAll(ri => ri.InventoryId.Equals(inventoryId));
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}