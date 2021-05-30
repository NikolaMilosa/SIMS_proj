using System;

namespace ZdravoHospital.GUI.DoctorUI.Exceptions
{
    public class IngredientAllergenException : Exception
    {
        private string _ingredientName;
        private string _medicineName;

        public IngredientAllergenException(string ingredientName, string medicineName)
        {
            _ingredientName = ingredientName;
            _medicineName = medicineName;
        }

        public override string Message => "Patient is allergic to an ingredient (" + _ingredientName + ")  in selected medicine (" + _medicineName + ").";
    }
}
