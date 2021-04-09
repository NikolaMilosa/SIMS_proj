using System;

namespace Model
{
    public class Doctor : Person
    {
        public string SpecialistType { get; set; }

        public Doctor(string name, string surname, string username, string specialistType)
        {
            Name = name;
            Surname = surname;
            Username = username;
            SpecialistType = specialistType;
        }
    }
}