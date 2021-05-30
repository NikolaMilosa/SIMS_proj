using System;

namespace ZdravoHospital.GUI.DoctorUI.Exceptions
{
    public class MedicineAllergenException : Exception
    {
        private string _medicineName;

        public MedicineAllergenException(string medicineName)
        {
            _medicineName = medicineName;
        }

        public override string Message => "Patient is allergic to selected medicine (" + _medicineName + ").";
    }
}
