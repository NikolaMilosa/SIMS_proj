using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using Newtonsoft.Json;

namespace Model.Repository
{
    public abstract class Repository<TKey, TValue>
    {
        private string path;
        protected static Mutex mutex = new Mutex();

        public Repository(string path)
        {
            this.path = path;
        }

        protected virtual void Save(List<TValue> values)
        {
            mutex.WaitOne();
            File.WriteAllText(path,JsonConvert.SerializeObject(values, Formatting.Indented));
            mutex.ReleaseMutex();
        }

        public virtual List<TValue> GetValues()
        {
            mutex.WaitOne();
            var values = JsonConvert.DeserializeObject<List<TValue>>(File.ReadAllText(path));

            if (values == null)
            {
                values = new List<TValue>();
            }
            mutex.ReleaseMutex();
            return values;
        }

        public abstract TValue GetById(TKey id);
        
        public abstract void DeleteById(TKey id);

        public abstract void Update(TValue newValue);

        public virtual void Create(TValue newValue)
        {
            mutex.WaitOne();
            var values = GetValues();

            values.Add(newValue);

            Save(values);
            mutex.ReleaseMutex();
        }

    }
}