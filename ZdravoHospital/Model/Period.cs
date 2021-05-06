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
        public PeriodMark PeriodMark { get; set; }
        public bool IsUrgent { get; set; }
        public Referral Referral { get; set; }
        

        public Period() 
        {

        }

        public Period(DateTime startTime, int duration, PeriodType periodType, string patientUsername, string doctorUsername, int roomId)
        {
            StartTime = startTime;
            Duration = duration;
            PeriodType = periodType;
            PatientUsername = patientUsername;
            DoctorUsername = doctorUsername;
            RoomId = roomId;
        }
       
        public bool HasPassed()
        {
            if (StartTime.AddMinutes(Duration) < DateTime.Now)
                return true;

            return false;
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