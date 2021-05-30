using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ShiftRule
    {
        public DateTime VacationStartTime { get; set; }
        public int NumberOfFreeDays { get; set; }
        public List<Vacation> Vacations { get; set; }
        public const int MaxFreeDays = 30;
        public Shift ScheduledShift { get; set; }
        public DateTime ShiftStart { get; set; }
        public Shift CurrentShift { get; set; }

        public ShiftRule(DateTime vacationStartTime, int numberOfFreeDays, Shift regularShift, DateTime shiftStart)
        {
            VacationStartTime = vacationStartTime;
            NumberOfFreeDays = numberOfFreeDays;
            ScheduledShift = regularShift;
            ShiftStart = shiftStart;
            if (ShiftStart.Date.Equals(DateTime.Now.Date))
                CurrentShift = regularShift;
        }

        public ShiftRule(DateTime vacationStartTime, int numberOfFreeDays, DateTime shiftStart)
        {
            VacationStartTime = vacationStartTime;
            NumberOfFreeDays = numberOfFreeDays;
            ShiftStart = shiftStart;
        }

        public ShiftRule()
        {

        }
    }
}
