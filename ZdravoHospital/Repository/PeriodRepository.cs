using System;
using System.Collections.Generic;
using System.Threading;

namespace Model.Repository
{
    public class PeriodRepository : Repository<int, Period>
    {
        private static string path = @"..\..\..\Resources\periods.json";

        public PeriodRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override Period GetById(int id)
        {
            var values = GetValues();
            return values.Find(value => value.PeriodId.Equals(id));
        }

        public override void DeleteById(int id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.PeriodId.Equals(id));
            Save(values);
        }

        public override void Update(Period newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.PeriodId.Equals(newValue.PeriodId))] = newValue;
            Save(values);
        }
    }
}