using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.ViewModel
{
    public class DoctorView
    {
        public string Fullname { get; set; }
        public string Username { get; set; }

        public DoctorView(Doctor doctor) 
        {
            Fullname = doctor.Name + " " + doctor.Surname;
            Username = doctor.Username;
        }
    }
}
