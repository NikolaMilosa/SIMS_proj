using System;

namespace Model
{
    public class City
    {
        public int PostalCode { get; set; }
        public string Name { get; set; }
        public Country Country { get; set; }

        public City()
        {
            Country = new Country();
        }
    }
}