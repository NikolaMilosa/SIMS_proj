using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Windows;

namespace Repository.PatientPersistance
{
    public class PatientRepository : IPatientRepository
    {
        private string _path = @"..\..\..\Resources\patients.json";
        public PatientRepository()
        {

        }

        public bool AddIngredientAllergenIfUnique(Patient patient, string newAllergen)
        {
            if (patient.IngredientAllergens == null) 
                patient.IngredientAllergens = new List<string>();
            var ingredient = patient.IngredientAllergens.Find(value => value.Equals(newAllergen));
            if (ingredient == null)
            {
                patient.IngredientAllergens.Add(newAllergen);
                Update(patient);
                return true;
            }
            else
            {
                return false;
            } 
        }

        public bool AddMedicineAllergenIfUnique(Patient patient, string newAllergen)
        {
            if (patient.MedicineAllergens == null)
                patient.MedicineAllergens = new List<string>();
            var medicine = patient.MedicineAllergens.Find(value => value.Equals(newAllergen));
            if (medicine == null)
            {
                patient.MedicineAllergens.Add(newAllergen);
                Update(patient);
                return true;
            }
            else
            {
                return false;
            }
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

        public bool RemoveIngredientAllergen(Patient patient, string allergen)
        {
            bool success = patient.IngredientAllergens.Remove(allergen);
            if(success)
                Update(patient);
            return success;
        }

        public bool RemoveMedicineAllergen(Patient patient, string allergen)
        {
            bool success = patient.MedicineAllergens.Remove(allergen);
            if(success)
                Update(patient);
            return success;
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