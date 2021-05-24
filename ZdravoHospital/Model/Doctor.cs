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
    }
}