using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI
{
    class EmptyStringValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value.ToString().Equals(String.Empty))
                    return new ValidationResult(false, "- Can't be empty...");
                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "- Unknown...");
            }
            
        }
    }
}
