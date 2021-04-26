using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class MedicineNameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim().ToLower();

            if (input.Equals(String.Empty))
            {
                return new ValidationResult(false, "- Can't be empty...");
            }

            if (!Regex.IsMatch(input, @"^([a-z]+(\s[a-z]+)*)$"))
            {
                return new ValidationResult(false, "- Wrong char...");
            }

            var doesExist = Model.Resources.medicines.Find(m => m.MedicineName.ToLower().Equals(input));

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
