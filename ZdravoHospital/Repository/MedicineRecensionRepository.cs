using System;
using System.Threading;

namespace Model.Repository
{
    public class MedicineRecensionRepository : Repository<string, MedicineRecension>
    {
        private static string path = @"..\..\..\Resources\medicineRecensions.json";

        public MedicineRecensionRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            return new Mutex();
        }

        public override MedicineRecension GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(MedicineRecension newValue)
        {
            throw new NotImplementedException();
        }
    }
}