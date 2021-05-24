using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTO
{
    public class CountryDTO
    {
        public string Name { get; set; }
        public CountryDTO(string name)
        {
            this.Name = name;
        }

        public CountryDTO() { }
    }
}
