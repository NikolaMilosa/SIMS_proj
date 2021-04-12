using System;

namespace Model
{
    public class Period
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public PeriodType PeriodType { get; set; }
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }
        public int RoomId { get; set; }
        public string Details { get; set; }

    }
}