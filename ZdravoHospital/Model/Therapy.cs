using System;

namespace Model
{
    public class Therapy
    {
        public Medicine Medicine { get; set; }
        public DateTime StartHours { get; set; }
        public int TimesPerDay { get; set; }
        public int PauseInDays { get; set; }
        public DateTime EndDate { get; set; }

    }
}