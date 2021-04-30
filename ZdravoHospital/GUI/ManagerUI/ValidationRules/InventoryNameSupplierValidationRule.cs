using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class InventoryNameSupplierValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim();

            if (input.Equals(string.Empty))
            {
                return new ValidationResult(false, "Red marked fields cannot be empty...");
            }

            if (!Regex.IsMatch(input, @"^([A-Za-z]+(\s[A-Za-z]+)*)$"))
            {
                return new ValidationResult(false, "You have entered an unsupported character...");
            }

            return new ValidationResult(true, null);
        }
    }
}
