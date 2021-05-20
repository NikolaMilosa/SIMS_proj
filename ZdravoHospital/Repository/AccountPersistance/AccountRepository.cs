using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Model.Repository
{
    public class AccountRepository : IAccountRepository
    {
        private static string path = @"..\..\..\Resources\accounts.json";

        public void Save(List<Credentials> values)
        {
            File.WriteAllText(path, JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public Credentials GetById(string key)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.Username.Equals(key))
                {
                    return val;
                }
            }

            return null;
        }

        public void DeleteById(string key)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Username.Equals(key));
            Save(values);
        }

        public void Update(Credentials newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }

        public List<Credentials> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Credentials>>(File.ReadAllText(path));
            return values;
        }

        public void Create(Credentials newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }
    }
}