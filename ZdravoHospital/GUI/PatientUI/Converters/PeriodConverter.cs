using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.Converters
{
    public class PeriodConverter
    {
        private DoctorFunctions doctorFunctions;
        private PeriodFunctions periodFunctions;

        public PeriodConverter()
        {
            doctorFunctions = new DoctorFunctions();
            periodFunctions = new PeriodFunctions();
        }
        public Period GetPeriod(PeriodDTO period)
        {
            return periodFunctions.GetPeriod(period.PeriodId);
        }
        public PeriodDTO GetPeriodDTO(Period period)
        {
            Doctor doctor = doctorFunctions.GetDoctor(period.DoctorUsername);//GetDoctor(period.DoctorUsername);
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
                DoctorUsername = doctorFunctions.GetDoctorUsername(periodDTO.DoctorName,periodDTO.DoctorSurname),
                RoomId = periodDTO.RoomNumber

            };
            return period;
        }

   

    }
}
