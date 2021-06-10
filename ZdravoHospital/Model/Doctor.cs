using System;
using System.Text.Json.Serialization;

namespace Model
{
    public class Doctor : Person
    {
        public Specialization SpecialistType { get; set; }
        public ShiftRule ShiftRule { get; set; }

        public Doctor(string name, string surname, string username, Specialization specialistType)
        {
            Name = name;
            Surname = surname;
            Username = username;
            SpecialistType = specialistType;
        }

        public override string ToString()
        {
            return Username + " | " + Name + " " + Surname;
        }

        public string Format()
        {
            string ret = "";

            ret += "BASIC INFO -";
            ret += "\n     Name : " + Name;
            ret += "\n     Surname : " + Surname;
            ret += "\n     Email : " + Email;
            ret += "\n     Date of birth : " + DateOfBirth.Date;
            ret += "\n     Phone number : " + PhoneNumber;
            ret += "\n     Username : " + Username;
            ret += "\n     Parents name : " + ParentsName;
           
            ret += "\n     Marital status : ";
            if (MaritalStatus == Model.MaritalStatus.DIVORCED)
                ret += "Divorced";
            else if (MaritalStatus == Model.MaritalStatus.MARRIED)
            {
                ret += "Married";
            }
            else if (MaritalStatus == Model.MaritalStatus.SINGLE)
            {
                ret += "Widowed";
            }
            else
            {
                ret += "Single";
            }

            ret += "\n     Gender : ";
            if (Gender == Model.Gender.FEMALE)
                ret += "Female";
            else
                ret += "Male";

            ret += "\n     Citizen ID : " + CitizenId;
            ret += "\n     Address : " + Address.StreetName + " " + Address.Number + ", " + Address.City.PostalCode + " " +
                   Address.City.Name + ", "
                   + Address.City.Country.Name;

            ret += "\n\nDOCTOR INFO -";
            ret += "\n     Specialization type : " + SpecialistType.SpecializationName;

            ret += "\n\nWORKING SHIFT -";
            ret += "\n     Current shift : " + ShiftRule.RegularShift;

            return ret;
        }
    }
}