using System;
using System.Text.Json.Serialization;

namespace Model
{
    public class Doctor : Person
    {
        public Specialization SpecialistType { get; set; }
        public ShiftRule ShiftRule { get; set; }
        public string NameSurnameSpecialization
        {
            get { return Name + " " + Surname + " (" + SpecialistType.SpecializationName + ")"; }
        }

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

            ret += "--------- BASIC INFO ---------";
            ret += "\nName : " + Name;
            ret += "\nSurname : " + Surname;
            ret += "\nEmail : " + Email;
            ret += "\nDate of birth : " + DateOfBirth.Date;
            ret += "\nPhone number : " + PhoneNumber;
            ret += "\nUsername : " + Username;
            ret += "\nParents name : " + ParentsName;
           
            ret += "\nMarital status : ";
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

            ret += "\nGender : ";
            if (Gender == Model.Gender.FEMALE)
                ret += "Female";
            else
                ret += "Male";

            ret += "\nCitizen ID : " + CitizenId;
            ret += "\nAddress : " + Address.StreetName + " " + Address.Number + ", " + Address.City.PostalCode + " " +
                   Address.City.Name + ", "
                   + Address.City.Country.Name;

            ret += "\n--------- DOCTOR INFO ---------";
            ret += "\nSpecialization type : " + SpecialistType.SpecializationName;

            return ret;
        }
    }
}