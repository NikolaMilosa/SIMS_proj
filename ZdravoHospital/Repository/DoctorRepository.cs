using System;
using System.Threading;

namespace Model.Repository
{
    public class DoctorRepository : Repository<string, Doctor>
    {
        private static string path = @"..\..\..\Resources\doctors.json";

        public DoctorRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override Doctor GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Doctor newValue)
        {
            throw new NotImplementedException();
        }
    }
}