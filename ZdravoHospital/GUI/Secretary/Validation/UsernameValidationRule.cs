using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.Secretary.Validation
{
    public class UsernameValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var text = value as string;
                if (text.Length == 0)
                    return new ValidationResult(false, "");
                Regex r = new Regex("^$|[a-zA-Z]+[a-zA-Z0-9_\\.\\s]*$");
                if (r.IsMatch(text))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Only letters, digits, dots and underscores.");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }

        }
    }
}
