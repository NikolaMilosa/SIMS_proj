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
      
        public AppointmentView(Period period)
        {
            Model.Resources.OpenDoctors();
            Period = period;
            DoctorName = Resources.doctors[period.DoctorUsername].Name;
            DoctorSurname= Resources.doctors[period.DoctorUsername].Surname;
        }
    }
}
