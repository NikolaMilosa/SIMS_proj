using System;
using System.Text;

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
        public Prescription Prescription { get; set; }
        public int Mark { get; set; }
        public bool IsUrgent { get; set; }
        

        public Period() { }

        public Period(DateTime startTime, int duration, PeriodType periodType, string patientUsername, string doctorUsername, int roomId, int prescriptionId)
        {
            StartTime = startTime;
            Duration = duration;
            PeriodType = periodType;
            PatientUsername = patientUsername;
            DoctorUsername = doctorUsername;
            RoomId = roomId;
        }

        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            builder.Append(StartTime.Hour.ToString());
            builder.Append(" : ");
            builder.Append(StartTime.Minute.ToString());
            builder.Append(" - ");
            DateTime endTime = StartTime.AddMinutes(Duration);
            builder.Append(endTime.Hour.ToString());
            builder.Append(" : ");
            builder.Append(endTime.Minute.ToString());
            builder.Append(" | ");
            builder.Append(RoomId);
            
            return builder.ToString();
        }
    }
}