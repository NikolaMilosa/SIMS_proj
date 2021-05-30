using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class VacationDTO
    {
        public DateTime VacationStartTime { get; set; }
        public int NumberOfFreeDays { get; set; }
        public VacationDTO()
        {
            VacationStartTime = DateTime.Today.Date;
        }
    }
}
