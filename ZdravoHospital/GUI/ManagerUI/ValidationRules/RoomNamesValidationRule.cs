using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class RoomNamesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string input = value as string;

            input = Regex.Replace(input, @"\s+", " ");
            input = input.Trim();

            if (input.Equals(String.Empty) || !Regex.IsMatch(input, @"^([a-zA-Z]+(\s([a-zA-Z]+|[1-9][0-9]*))*)$"))
            {
                return new ValidationResult(false, null);
            }

            return new ValidationResult(true, null);
        }
    }
}
