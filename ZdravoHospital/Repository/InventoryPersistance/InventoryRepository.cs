using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Repository.InventoryPersistance
{
    public class InventoryRepository : IInventoryRepository
    {
        private static string _path = @"..\..\..\Resources\inventory.json";
        private static Mutex _mutex;

        public InventoryRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void Save(List<Inventory> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public Inventory GetById(string id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Id.Equals(id))
                {
                    return val;
                }
            }

            return null;
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void Update(Inventory newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public List<Inventory> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<Inventory>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Inventory>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Create(Inventory newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public Inventory GetByName(string name)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Name.Equals(name))
                {
                    return val;
                }
            }

            return null;
        }
    }
}