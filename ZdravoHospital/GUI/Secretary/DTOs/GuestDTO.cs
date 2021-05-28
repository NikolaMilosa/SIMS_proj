using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class GuestDTO
    {
        public GuestDTO(bool urgentlyCreated)
        {
            UrgentlyCreated = urgentlyCreated;
        }

        public string Name { get; set; }
        public string Surname { get; set; }

        public string CitizenId { get; set; }

        public string HealthCardNumber { get; set; }
        public bool UrgentlyCreated { get; set; }
        public string Username { get; set; }
    }
}
