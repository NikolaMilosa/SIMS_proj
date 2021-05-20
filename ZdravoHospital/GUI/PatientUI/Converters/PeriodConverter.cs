using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.DTOs;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.Converters
{
    public class PeriodConverter
    {
        public Model.Period GetPeriod(PeriodDTO period)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            return periodRepository.GetById(period.PeriodId);
        }
        public PeriodDTO GetPeriodDTO(Period period)
        {
            Doctor doctor = GetDoctor(period.DoctorUsername);
            return new PeriodDTO(doctor.Name, doctor.Surname, period.StartTime, period.RoomId, period.PeriodType,
                period.PeriodId);
        }

        private Doctor GetDoctor(string username)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            return doctorRepository.GetById(username);
        }

    }
}
