using System;
using System.Threading;

namespace Model.Repository
{
    public class AccountRepository : Repository<string, Credentials>
    {
        private static string path = @"..\..\..\Resources\accounts.json";

        private static Mutex mutex;

        public AccountRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override Credentials GetById(string id)
        {
            var values = GetValues();
            return values.Find(value => value.Username.Equals(id));
        }

        public override void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.Username.Equals(id));
            Save(values);
        }

        public override void Update(Credentials newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Username.Equals(newValue.Username))] = newValue;
            Save(values);
        }
    }
}