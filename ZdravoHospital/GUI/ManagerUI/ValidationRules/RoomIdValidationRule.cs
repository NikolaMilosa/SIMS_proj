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
                if (value.ToString().Trim().Equals(String.Empty))
                    return new ValidationResult(false, "'Id' field cannot be empty...");

                int id = int.Parse(value.ToString());

                if (Model.Resources.rooms.ContainsKey(id))
                    return new ValidationResult(false, "Room with that Id already exists...");

                return new ValidationResult(true, null);
            }
            catch
            {
                return new ValidationResult(false, "'Id' accepts only digits...");
            }
        }
    }
}
