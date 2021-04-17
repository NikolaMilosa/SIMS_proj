using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.Secretary.Validation
{
    public class LettersAndSpacesValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var text = value as string;
                if(text.Length == 0)
                    return new ValidationResult(false, "");

                Regex r = new Regex("^[a-zA-Z-\\s]+$");
                if (r.IsMatch(text))
                {
                    return new ValidationResult(true, null);
                }
                return new ValidationResult(false, "Only letters, spaces and hyphens.");
            }
            catch
            {
                return new ValidationResult(false, "Unknown error occured.");
            }

        }
    }
}
