using System;
using System.Collections.Generic;

namespace Model
{
    public class Patient : Person
    {
        public bool IsGuest { get; set; }
        public string HealthCardNumber { get; set; }
        public BloodType BloodType { get; set; }
        public List<Medicine> MedicineAllergens { get; set; }
        public List<Ingredient> IngredientAllergens { get; set; }

        public System.Collections.Generic.List<Prescription> Prescription { get; set; }
    }
}