using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.Logics
{
    public class PrescriptionLogic
    {
        public PrescriptionLogic()
        {

        }

        public void GenerateTherapies(Prescription prescription, ObservableCollection<Therapy> therapies)
        {
            prescription.TherapyList = new List<Therapy>();

            foreach (Therapy t in therapies)
                prescription.TherapyList.Add(t);
        }

        public void SaveChanges() // prescription is saved within period
        {
            Model.Resources.SavePeriods();
        }

        public bool IsPatientAllergicToMedicine(Patient patient, Medicine medicine)
        {
            foreach (string medicineAllergen in patient.MedicineAllergens)
                if (medicine.MedicineName.Equals(medicineAllergen))
                    return true;

            return false;
        }

        public Ingredient DetectIngredientAllegren(Patient patient, Medicine medicine)
        {
            foreach (string ingredientAllergen in patient.IngredientAllergens)
                foreach (Ingredient ingredient in medicine.Ingredients)
                    if (ingredient.IngredientName.Equals(ingredientAllergen))
                        return ingredient;

            return null;
        }
    }
}
