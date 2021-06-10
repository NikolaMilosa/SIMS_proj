using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class ShiftDTO
    {
        public Shift ScheduledShift { get; set; }
        public DateTime ShiftStart { get; set; }
        public bool IsSingleDayShift { get; set; }

        public ShiftDTO(Shift scheduledShift, DateTime shiftStart, bool isSingleDayShift)
        {
            ScheduledShift = scheduledShift;
            ShiftStart = shiftStart;
            IsSingleDayShift = isSingleDayShift;
        }
        public ShiftDTO()
        {
            ShiftStart = DateTime.Today.AddDays(1).Date;
        }
    }
}
