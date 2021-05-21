using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace Repository.PatientPersistance
{
    public class PatientRepository : IPatientRepository
    {
        private string _path = @"..\..\..\Resources\patients.json";
        public PatientRepository()
        {

        }
        public void Create(Patient newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.Username.Equals(id));
            Save(values);
        }

        public Patient GetById(string id)
        {
            List<Patient> patients = GetValues();
            foreach (Patient patient in patients)
                if (patient.Username.Equals(id))
                    return patient;

            return null;
        }

        public List<Patient> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Patient>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Patient>();
            }

            return values;
        }

        public void Save(List<Patient> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Patient newValue)
        {
            List<Patient> patients = GetValues();
            patients[patients.FindIndex(patient => patient.Username.Equals(newValue.Username))] = newValue;
            Save(patients);
        }
    }
}