using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.ViewModel
{
    public class AppointmentView : Period
    { 
        public string DoctorName { get; set; }
        public string DoctorSurname { get; set; }
    }
}
