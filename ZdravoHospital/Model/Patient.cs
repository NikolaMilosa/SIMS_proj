using System;
using System.Collections.Generic;

namespace Model
{
    public class Patient : Person
    {
        public bool IsGuest { get; set; }
        public string HealthCardNumber { get; set; }
        public BloodType BloodType { get; set; }
        public List<Medicine> MedicineAllergens { get; set; }
        public List<Ingredient> IngredientAllergens { get; set; }

        public System.Collections.Generic.List<Prescription> Prescription { get; set; }

        public Patient(string healthCardNum, string name, string surname, string email, DateTime dateOfBirth, string phoneNumber, string username, string parentsName, MaritalStatus maritalStatus, Gender gender, string personID, BloodType bloodType)
        {
            Name = name;
            Surname = surname;
            Email = email;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Username = username;
            ParentsName = parentsName;
            MaritalStatus = maritalStatus;
            Gender = gender;
            HealthCardNumber = healthCardNum;
            CitizenId = personID;
            BloodType = bloodType;
        }
    }
}