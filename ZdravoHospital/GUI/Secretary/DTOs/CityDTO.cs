using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTO
{
    public class CityDTO
    {
        public int PostalCode { get; set; }
        public string Name { get; set; }
        public CountryDTO CountryDTO { get; set; }
        public CityDTO(int postalCode, string name, CountryDTO country)
        {
            this.PostalCode = postalCode;
            this.Name = name;
            this.CountryDTO = country;
        }

        public CityDTO()
        {
            this.CountryDTO = new CountryDTO();
        }
    }
}
