using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Model.Repository
{
    public abstract class Repository<TKey, TValue>
    {
        private List<TValue> values;
        private string path;

        public Repository(string path)
        {
            path = this.path;
            values = JsonConvert.DeserializeObject<List<TValue>>(File.ReadAllText(path));
        }

        protected virtual void Save()
        {
            File.WriteAllText(path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public virtual List<TValue> GetValues() => values;

        public abstract TValue GetById(TKey id);

        public virtual void Delete(TValue value)
        {
            values.Remove(value);
            Save();
        }

        public abstract void DeleteById(TKey id);

        public abstract void Update(TValue newValue);

        public virtual void Create(TValue newValue)
        {
            values.Add(newValue);
            Save();
        }

    }
}