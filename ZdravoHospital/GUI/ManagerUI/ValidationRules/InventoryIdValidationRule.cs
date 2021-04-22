using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class InventoryIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string id = value.ToString();

                if (id.Equals(String.Empty))
                    return new ValidationResult(false, "- Can't be empty...");

                if (Model.Resources.inventory.ContainsKey(id))
                    return new ValidationResult(false, "- Id exists...");

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "- Unknown...");
            }
        }
    }
}
