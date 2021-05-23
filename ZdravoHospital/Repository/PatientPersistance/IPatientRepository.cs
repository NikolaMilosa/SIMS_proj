using Model;
using System;

namespace Repository.PatientPersistance
{
   public interface IPatientRepository : IRepository<string, Patient>
   {
        bool AddMedicineAllergenIfUnique(Model.Patient patient, string newAllergen);
        bool AddIngredientAllergenIfUnique(Model.Patient patient, string newAllergen);
        bool RemoveMedicineAllergen(Model.Patient patient, string allergen);
        bool RemoveIngredientAllergen(Model.Patient patient, string allergen);
    }
}