using System;

namespace Model.Repository
{
    public class ReferralRepository : Repository<int, Referral>
    {
        private static string path = @"..\..\..\Resources\referrals.json";

        public ReferralRepository() : base(path)
        {
        }

        public override Referral GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Referral newValue)
        {
            throw new NotImplementedException();
        }
    }
}