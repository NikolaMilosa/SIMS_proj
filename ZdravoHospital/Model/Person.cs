using System;

namespace Model
{
    public class Person
    {
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string PhoneNumber { get; set; }
        public string Username { get; set; }
        public string ParentsName { get; set; }
        public MaritalStatus? MaritalStatus { get; set; }
        public Gender? Gender { get; set; }
        public string CitizenId { get; set; }
        public Address Address { get; set; }

        public Person() 
        {
            Address = new Address();
        }
    }
}