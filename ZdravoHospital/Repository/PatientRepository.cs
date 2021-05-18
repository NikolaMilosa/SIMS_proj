using System;
using System.Collections.Generic;

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
            List<Patient> patients = GetValues();
            foreach(Patient patient in patients)
                if (patient.Username.Equals(id))
                    return patient;

            return null;
        }

        public override void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public override void Update(Patient newValue)
        {
            Save();
        }
    }
}