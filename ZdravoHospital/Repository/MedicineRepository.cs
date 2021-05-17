using System;

namespace Model.Repository
{
    public class MedicineRepository : Repository<string, Medicine>
    {
        private static string path = @"..\..\..\Resources\medicines.json";

        public MedicineRepository() : base(path)
        {
        }

        public override Medicine GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Medicine newValue)
        {
            throw new NotImplementedException();
        }
    }
}