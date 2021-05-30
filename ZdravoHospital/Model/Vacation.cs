using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class Vacation
    {
        public DateTime VacationStartTime { get; set; }
        public int NumberOfFreeDays { get; set; }

        public Vacation(DateTime vacationStartTime, int numberOfFreeDays)
        {
            VacationStartTime = vacationStartTime;
            NumberOfFreeDays = numberOfFreeDays;
        }
    }
}
