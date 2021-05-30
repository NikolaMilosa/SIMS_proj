using Model;
using Repository.DoctorPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class VacationService
    {
        private IDoctorRepository _doctorRepository;
        public VacationService()
        {
            _doctorRepository = new DoctorRepository();
        }

        public void ProcessVacationCreation(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            selectedDoctor.ShiftRule.Vacations.Add(new Vacation(vacationDTO.VacationStartTime, vacationDTO.NumberOfFreeDays));
            _doctorRepository.Update(selectedDoctor);
        }

        public void ProcessVacationDeletion(Doctor selectedDoctor)
        {
            selectedDoctor.ShiftRule.Vacations.Clear();
            _doctorRepository.Update(selectedDoctor);
        }
    }
}
