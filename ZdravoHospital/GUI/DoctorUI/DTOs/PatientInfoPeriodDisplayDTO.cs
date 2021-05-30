using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.DTOs
{
    public class PatientInfoPeriodDisplayDTO
    {
        public Period Period { get; }
        public Doctor Doctor { get; }

        public PatientInfoPeriodDisplayDTO(Period period, Doctor doctor)
        {
            Period = period;
            Doctor = doctor;
        }
    }
}
