using System;

namespace Model
{
    public class Address
    {
        public string StreetName { get; set; }
        public string Number { get; set; }
        public City City { get; set; }
        public Address()
        {
            City = new City();
        }

    }
}