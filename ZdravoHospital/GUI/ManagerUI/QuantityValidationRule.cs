using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI
{
    class QuantityValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value is int)
            {
                if ((int)value < 1)
                    return new ValidationResult(false, "- Atleast one...");
                return new ValidationResult(true, null);
            }
            else
            {
                return new ValidationResult(false, "- Only digits...");
            }
        }
    }
}
