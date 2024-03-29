using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Repository.CredentialsPersistance
{
    public class CredentialsRepository : ICredentialsRepository
    {
        private string _path = @"..\..\..\Resources\credentials.json";
        private Mutex _mutex;

        public CredentialsRepository()
        {
            _mutex = new Mutex();
        }

        public void Create(Credentials newValue)
        {
            _mutex.WaitOne();
            var values = GetValues();

            values.Add(newValue);

            Save(values);
            _mutex.ReleaseMutex();
        }

        public bool CreateIfUnique(Credentials newValue)
        {
            var values = GetValues();
            Credentials existingAccount = values.Find(value => value.Username.Equals(newValue.Username));
            if (existingAccount == null)
            {
                values.Add(newValue);
                Save(values);
                return true;
            }  

            return false;
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.Username.Equals(id));
            Save(values);
        }

        public Credentials GetById(string id)
        {
            var values = GetValues();
            return values.Find(value => value.Username.Equals(id));
        }

        public List<Credentials> GetValues()
        {
            _mutex.WaitOne();
            var values = JsonConvert.DeserializeObject<List<Credentials>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Credentials>();
            }

            _mutex.ReleaseMutex();
            return values;
        }

        public void Save(List<Credentials> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Credentials newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }
    }
}