using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.Converters
{
    public class PeriodConverter
    {
        private DoctorService doctorFunctions;
        private PeriodService periodFunctions;

        public PeriodConverter()
        {
            doctorFunctions = new DoctorService();
            periodFunctions = new PeriodService();
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
