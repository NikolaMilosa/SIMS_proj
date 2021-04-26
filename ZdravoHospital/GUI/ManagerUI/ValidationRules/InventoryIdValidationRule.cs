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
                string id = value.ToString();

                if (id.Equals(String.Empty))
                    return new ValidationResult(false, "- Can't be empty...");

                string regex = @"[a-zA-Z0-9]+";

                if (!Regex.IsMatch(id, regex))
                    return new ValidationResult(false, " - Wrong char...");

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
