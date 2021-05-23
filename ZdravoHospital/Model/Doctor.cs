using System;

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
    }
}