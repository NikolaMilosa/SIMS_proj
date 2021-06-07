using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class WeeklyReportDTO
    {
        public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }
        public PeriodType PeriodType { get; set; }
        public int RoomID { get; set; }

        public WeeklyReportDTO(DateTime startTime, int duration, string patientUsername, string doctorUsername, PeriodType periodType, int roomID)
        {
            StartTime = startTime;
            Duration = duration;
            PatientUsername = patientUsername;
            DoctorUsername = doctorUsername;
            PeriodType = periodType;
            RoomID = roomID;
        }

        public WeeklyReportDTO()
        {

        }

        public string Format()
        {
            string ret = "";

            ret += "   Start time : " + StartTime.ToShortDateString() + " " + StartTime.ToShortTimeString();
            ret += "\n     Duration : " + Duration;
            ret += "\n     Patient username : " + PatientUsername;
            ret += "\n     Doctor username : " + DoctorUsername;
            ret += "\n     Period type : " + PeriodType;
            ret += "\n     Room : " + RoomID;

            return ret;
        }
    }
}
