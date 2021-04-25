using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class MedicineNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            var doesExist = Model.Resources.medicines.Find(m => m.MedicineName.ToLower().Equals(input.Trim().ToLower()));

            if (doesExist == null)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "- Already exists...");
            }
        }
    }
}
