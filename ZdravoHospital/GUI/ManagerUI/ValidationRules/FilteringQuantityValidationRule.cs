using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class FilteringQuantityValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value.ToString().Equals(String.Empty))
                return new ValidationResult(true, null);

            int enteredQuantity;

            if(int.TryParse(value.ToString(),out enteredQuantity))
            {
                if (enteredQuantity <= 0)
                {
                    return new ValidationResult(false, "Quantity needs to be at least 1 or more...");
                }

                return new ValidationResult(true, null);
            }

            return new ValidationResult(false, "Quantity can only accept digits...");
        }
    }
}
