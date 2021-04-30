using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Controls;
namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class InventoryIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                var id = value.ToString();

                if (id.Equals(string.Empty))
                    return new ValidationResult(false, "Id cannot be empty...");

                var regex = @"^[a-zA-Z0-9]+$";

                if (!Regex.IsMatch(id, regex))
                    return new ValidationResult(false, "In id you have entered an unsupported character...");

                if (Model.Resources.inventory.ContainsKey(id))
                    return new ValidationResult(false, "Inventory with that Id already exists...");

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "Unknown...");
            }
        }
    }
}
