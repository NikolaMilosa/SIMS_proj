using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.ViewModel
{
    public class AppointmentView 
    { 
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
        public Period Period { get; set; }
        /*
         public DateTime StartTime { get; set; }
        public int Duration { get; set; }
        public PeriodType PeriodType { get; set; }
        public string PatientUsername { get; set; }
        public string DoctorUsername { get; set; }
        public int RoomId { get; set; }
        public string Details { get; set; }
         */
        public AppointmentView(Period period)
        {
            Model.Resources.DeserializeDoctors();
            Period = period;
            DoctorName = Resources.doctors[period.DoctorUsername].Name;
            DoctorSurname= Resources.doctors[period.DoctorUsername].Surname;
        }
    }
}
