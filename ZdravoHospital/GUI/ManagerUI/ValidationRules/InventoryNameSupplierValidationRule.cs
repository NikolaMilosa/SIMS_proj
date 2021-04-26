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
            string input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim();

            if (!Regex.IsMatch(input, @"^([A-Za-z]+(\s[A-Za-z]+)*)$"))
            {
                return new ValidationResult(false, null);
            }

            return new ValidationResult(true, null);
        }
    }
}
