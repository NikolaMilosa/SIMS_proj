using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class FilteringInventoryIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim();

            if (!Regex.IsMatch(input, @"^([A-Za-z0-9]*)$"))
            {
                return new ValidationResult(false, "You have entered an unsupported character...");
            }

            return new ValidationResult(true, null);
        }
    }
}
