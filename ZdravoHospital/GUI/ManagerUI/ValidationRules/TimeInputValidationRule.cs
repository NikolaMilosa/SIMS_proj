using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class TimeInputValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        { 
            return new ValidationResult(true, null);
        }
    }
}
