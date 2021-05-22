using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.Text;

namespace Model
{
    public class Period
    {
        public int PeriodId { get; set; }
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
        public int ReferringReferralId { get; set; }
        public int ReferredReferralId { get; set; }
        public Treatment Treatment { get; set; }

        [JsonIgnore]
        public ObservableCollection<Model.MovePeriod> MovePeriods { get; set; }
        

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
            ReferredReferralId = -1;
            ReferringReferralId = -1;
        }
       
        public bool HasPassed()
        {
            if (StartTime.AddMinutes(Duration) < DateTime.Now)
                return true;

            return false;
        }

        // for urgent periods
        public Period(DateTime startTime, int duration, string patientUsername, string doctorUsername, bool isUrgent)
        {
            StartTime = startTime;
            Duration = duration;
            PatientUsername = patientUsername;
            DoctorUsername = doctorUsername;
            IsUrgent = isUrgent;
            MovePeriods = new ObservableCollection<MovePeriod>();
            ReferredReferralId = -1;
            ReferringReferralId = -1;   
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

        public int findSumOfMovePeriods()
        {
            int ret = 0;
            foreach(var movePeriod in MovePeriods)
            {
                ret += (int)movePeriod.MovedStartTime.Subtract(movePeriod.InitialStartTime).TotalMinutes;
            }
            return ret;
        }
    }
}