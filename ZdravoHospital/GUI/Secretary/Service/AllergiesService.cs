using Model;
using Repository.MedicinePersistance;
using Repository.PatientPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.Repository.IngredientPersistance;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class AllergiesService
    {
        private IIngredientRepository _ingredientRepository;
        private IMedicineRepository _medicineRepository;
        private IPatientRepository _patientRepository;
        public AllergiesService()
        {
            _ingredientRepository = new IngredientRepository();
            _medicineRepository = new MedicineRepository();
            _patientRepository = new PatientRepository();
        }
        public List<Medicine> GetAllMedicines()
        {
            return _medicineRepository.GetValues();
        }
        public List<Ingredient> GetAllIngredients()
        {
            return _ingredientRepository.GetValues();
        }

        public bool AddMedicalAllergen(Patient patient, string selectedAllergen)
        {
            return _patientRepository.AddMedicineAllergenIfUnique(patient, selectedAllergen);
        }

        public bool AddIngredientAllergen(Patient patient, string selectedAllergen)
        {
            return _patientRepository.AddIngredientAllergenIfUnique(patient, selectedAllergen);
        }

        public bool RemoveMedicineAllergen(Patient patient, string selectedAllergen)
        {
            return _patientRepository.RemoveMedicineAllergen(patient, selectedAllergen);
        }
        public bool RemoveIngredientAllergen(Patient patient, string selectedAllergen)
        {
            return _patientRepository.RemoveIngredientAllergen(patient, selectedAllergen);
        }
        public bool IsAllergenMedicine(Patient patient, string allergen)
        {
            if (patient.MedicineAllergens.Contains(allergen))
                return true;
            return false;
        }
    }
}
