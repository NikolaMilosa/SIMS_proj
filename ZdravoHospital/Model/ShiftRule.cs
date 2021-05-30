using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Model
{
    public class ShiftRule
    {
        public List<Vacation> Vacations { get; set; }
        public const int MaxFreeDays = 30;
        public DoctorsShift RegularShift { get; set; }
        public List<DoctorsShift> SingleDayShifts { get; set; }

        public ShiftRule()
        {

        }
    }
}
