using System;

namespace Model.Repository
{
    public class PatientRepository : Repository<string, Patient>
    {
        private static string path = @"..\..\..\Resources\patients.json";

        public PatientRepository() : base(path)
        {
        }

        public override Patient GetById(string id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Patient newValue)
        {
            throw new NotImplementedException();
        }
    }
}