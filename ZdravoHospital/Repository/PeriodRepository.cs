using System;

namespace Model.Repository
{
    public class PeriodRepository : Repository<int, Period>
    {
        private static string path = @"..\..\..\Resources\periods.json";

        public PeriodRepository() : base(path)
        {
        }

        public override Period GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Period newValue)
        {
            throw new NotImplementedException();
        }
    }
}