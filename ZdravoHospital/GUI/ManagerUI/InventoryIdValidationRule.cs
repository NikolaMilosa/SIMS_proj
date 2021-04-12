using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;

namespace ZdravoHospital.GUI.ManagerUI
{
    class InventoryIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                string id = value.ToString();

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
