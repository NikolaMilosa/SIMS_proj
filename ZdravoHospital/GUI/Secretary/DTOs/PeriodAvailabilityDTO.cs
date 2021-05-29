using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class PeriodAvailabilityDTO
    {
        public int PeriodId { get; set; }
        public PeriodAvailability PeriodAvailable { get; set; }

        public PeriodAvailabilityDTO(int periodId, PeriodAvailability periodAvailable)
        {
            PeriodId = periodId;
            PeriodAvailable = periodAvailable;
        }
        public PeriodAvailabilityDTO()
        {

        }
    }
}
