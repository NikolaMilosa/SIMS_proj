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
            var input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim().ToLower();

            if (input.Equals(string.Empty))
            {
                return new ValidationResult(false, "'Name' cannot be empty...");
            }

            if (!Regex.IsMatch(input, @"^([a-z]+(\s[a-z]+)*)$"))
            {
                return new ValidationResult(false, "In 'Name' you have entered an unsupported character...");
            }

            var doesExist = Model.Resources.medicines.Find(m => m.MedicineName.ToLower().Equals(input));

            if (doesExist == null)
            {
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "Medicine with that name already exists...");
            }
        }
    }
}
