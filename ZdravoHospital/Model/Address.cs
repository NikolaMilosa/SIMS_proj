using System;

namespace Model
{
    public class Address
    {
        public string StreetName { get; set; }
        public string Number { get; set; }

        public City City { get; set; }
        public Address(string streetName, string number, City city)
        {
            this.StreetName = streetName;
            this.Number = number;
            this.City = city;
        }

        public Address()
        {
            this.City = new City();
        }

        public override string ToString()
        {
            return StreetName + " " + Number + ", " + City.PostalCode + " " + City.Name + ", " + City.Country.Name;
        }
    }
}