using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Windows.Controls;
using Repository.RoomPersistance;

namespace ZdravoHospital.GUI.ManagerUI.ValidationRules
{
    class RoomIdValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            try
            {
                if (value.ToString().Trim().Equals(string.Empty))
                    return new ValidationResult(false, "'Id' field cannot be empty...");

                var id = int.Parse(value.ToString());

                RoomRepository roomRepo = new RoomRepository();

                if (roomRepo.GetById(id) != null)
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
