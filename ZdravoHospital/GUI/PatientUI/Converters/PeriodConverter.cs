using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.ViewModels;
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

        public Period GeneratePeriod(PeriodDTO periodDTO)
        {
            Period period = new Period
            {
                PatientUsername = PatientWindowVM.PatientUsername,
                Duration = 30,
                PeriodId = periodDTO.PeriodId,
                StartTime = periodDTO.Date,
                PeriodType = periodDTO.PeriodType,
                DoctorUsername = GetDoctorUsername(periodDTO.DoctorName,periodDTO.DoctorSurname),
                RoomId = periodDTO.RoomNumber

            };
            return period;
        }

        private Doctor GetDoctor(string username)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            return doctorRepository.GetById(username);
        }

        private string GetDoctorUsername(string name,string surname)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            return (from doctor in doctorRepository.GetValues() where doctor.Name.Equals(name) && doctor.Surname.Equals(surname) select doctor.Username).FirstOrDefault();
        }

    }
}
