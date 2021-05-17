using System;

namespace Model.Repository
{
    public class AccountRepository : Repository<string, Credentials>
    {
        private static string path = @"..\..\..\Resources\accounts.json";

        public AccountRepository() : base(path)
        {
        }

        public override Credentials GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Credentials newValue)
        {
            throw new NotImplementedException();
        }
    }
}