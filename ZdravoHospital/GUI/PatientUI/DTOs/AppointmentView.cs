using Model;
using Model.Repository;
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
            DoctorRepository doctorRepository = new DoctorRepository();
            Period = period;
            Doctor doctor = doctorRepository.GetById(period.DoctorUsername);
            DoctorName = doctor.Name;
            DoctorSurname= doctor.Surname;
        }
    }
}
