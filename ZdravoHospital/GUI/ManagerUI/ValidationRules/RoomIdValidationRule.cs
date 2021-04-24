using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class RoomIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                int id = int.Parse(value.ToString());

                if (Model.Resources.rooms.ContainsKey(id))
                    return new ValidationResult(false, "- Id exists...");

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "- Only digits...");
            }
        }
    }
}
