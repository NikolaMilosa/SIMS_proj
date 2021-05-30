using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class DoctorsShift
    {
        public Shift ScheduledShift { get; set; }
        public DateTime ShiftStart { get; set; }
        public bool IsSingleDayShift { get; set; }

        public DoctorsShift(Shift scheduledShift, DateTime shiftStart, bool isSingleDayShift)
        {
            ScheduledShift = scheduledShift;
            ShiftStart = shiftStart;
            IsSingleDayShift = isSingleDayShift;
        }
    }
}
