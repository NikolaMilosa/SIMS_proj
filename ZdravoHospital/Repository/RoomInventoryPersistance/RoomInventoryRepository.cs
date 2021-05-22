using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading;
using Accessibility;
using Newtonsoft.Json;

namespace Repository.RoomInventoryPersistance
{
    public class RoomInventoryRepository : IRoomInventoryRepository
    {
        private static string _path = @"..\..\..\Resources\roomInventory.json";
        private static Mutex _mutex;

        public RoomInventoryRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void Create(RoomInventory newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteById(int id)
        {
            throw new NotSupportedException();
        }

        public RoomInventory GetById(int id)
        {
            throw new NotSupportedException();
        }

        public List<RoomInventory> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<RoomInventory>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<RoomInventory>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<RoomInventory> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(RoomInventory newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.RoomId == newValue.RoomId &&
                                           val.InventoryId.Equals(newValue.InventoryId))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public RoomInventory FindByBothIds(int roomId, string inventoryId)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.RoomId == roomId && val.InventoryId.Equals(inventoryId))
                {
                    return val;
                }
            }

            return null;
        }

        public void DeleteByEquality(RoomInventory roomInventory)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.RoomId == roomInventory.RoomId &&
                                    val.InventoryId.Equals(roomInventory.InventoryId));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteByInventoryId(string inventoryId)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.InventoryId.Equals(inventoryId));
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}