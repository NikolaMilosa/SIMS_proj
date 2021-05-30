using Model;
using ZdravoHospital.GUI.DoctorUI.Exceptions;

namespace ZdravoHospital.GUI.DoctorUI.Validations
{
    public class PrescriptionValidation
    {
        public void CheckAllergens(Medicine medicine, Patient patient)
        {
            CheckMedicineAllergens(medicine, patient);
            CheckIngredientAllergens(medicine, patient);
        }

        private void CheckMedicineAllergens(Medicine medicine, Patient patient)
        {
            foreach (string medicineAllergen in patient.MedicineAllergens)
                if (medicine.MedicineName.Equals(medicineAllergen))
                    throw new MedicineAllergenException(medicine.MedicineName);
        }

        private void CheckIngredientAllergens(Medicine medicine, Patient patient)
        {
            foreach (string ingredientAllergen in patient.IngredientAllergens)
                foreach (Ingredient ingredient in medicine.Ingredients)
                    if (ingredient.IngredientName.Equals(ingredientAllergen))
                        throw new IngredientAllergenException(ingredient.IngredientName, medicine.MedicineName);
        }
    }
}
