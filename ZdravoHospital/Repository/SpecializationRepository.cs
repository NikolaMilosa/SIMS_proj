using System;
using System.Threading;

namespace Model.Repository
{
    public class SpecializationRepository : Repository<string, Specialization>
    {
        private static string path = @"..\..\..\Resources\specializations.json";

        public SpecializationRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override Specialization GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Specialization newValue)
        {
            throw new NotImplementedException();
        }
    }
}