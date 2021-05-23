using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTO
{
    public class AddressDTO
    {
        public string StreetName { get; set; }
        public string Number { get; set; }

        public CityDTO CityDTO { get; set; }
        public AddressDTO(string streetName, string number, CityDTO city)
        {
            this.StreetName = streetName;
            this.Number = number;
            this.CityDTO = city;
        }

        public AddressDTO()
        {
            this.CityDTO = new CityDTO();
        }
    }
}
