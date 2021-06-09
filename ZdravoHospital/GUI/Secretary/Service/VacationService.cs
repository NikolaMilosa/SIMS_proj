using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class VacationService
    {
        private IDoctorRepository _doctorRepository;
        private IPeriodRepository _periodRepository;
        public WorkTimeService WorkService;
        public VacationService()
        {
            _doctorRepository = new DoctorRepository();
            _periodRepository = new PeriodRepository();
            WorkService = new WorkTimeService();
        }

        public void ProcessVacationCreation(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            selectedDoctor.ShiftRule.Vacations.Add(new Vacation(vacationDTO.VacationStartTime, vacationDTO.NumberOfFreeDays));
            _doctorRepository.Update(selectedDoctor);
            moveAffectedPeriods(vacationDTO, selectedDoctor);
        }

        public void ProcessVacationDeletion(Doctor selectedDoctor)
        {
            selectedDoctor.ShiftRule.Vacations.Clear();
            _doctorRepository.Update(selectedDoctor);
        }

        private List<Period> getScheduledPeriodsToMove(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            DateTime vacationEnd = vacationDTO.VacationStartTime.AddDays(vacationDTO.NumberOfFreeDays);
            List<Period> scheduledPeriods = new List<Period>();
            foreach(var period in allPeriods)
            {
                if(period.DoctorUsername == selectedDoctor.Username)
                {
                    if (period.StartTime.Date >= vacationDTO.VacationStartTime.Date && period.StartTime.Date <= vacationEnd.Date)
                    {
                        scheduledPeriods.Add(period);
                    }
                }
            }
            return scheduledPeriods;
        }

        private void moveAffectedPeriods(VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            List<Period> affectedPeriods = getScheduledPeriodsToMove(vacationDTO, selectedDoctor);
            foreach(var period in affectedPeriods)
            {
                period.StartTime = findFreeSpot(period, vacationDTO, selectedDoctor);
                _periodRepository.Update(period);
            }
        }

        private DateTime findFreeSpot(Period period, VacationDTO vacationDTO, Doctor selectedDoctor)
        {
            DateTime doctorBackToWorkTime = vacationDTO.VacationStartTime.AddDays(vacationDTO.NumberOfFreeDays + 1);
            DateTime startTimeSearch = WorkService.getDoctorsShiftStartTime(selectedDoctor, doctorBackToWorkTime);
            period.StartTime = startTimeSearch;
            while(!isDoctorFreeAtCertainTime(period) || !isPatientFreeAtCertainTime(period) || !isRoomFreeAtCertainTime(period))
            {
                period.StartTime = period.StartTime.AddMinutes(1);
            }
            return period.StartTime;
        }
        private bool periodsOverlap(Period newPeriod, Period existingPeriod)
        {
            DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);
            DateTime newPeriodEndtime = newPeriod.StartTime.AddMinutes(newPeriod.Duration);
            if (newPeriod.StartTime < existingPeriodEndTime && newPeriodEndtime > existingPeriod.StartTime)
            {
                return true;
            }
            return false;
        }

        private bool isDoctorFreeAtCertainTime(Period selectedPeriod)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            foreach(var period in allPeriods)
            {
                if(period.DoctorUsername == selectedPeriod.DoctorUsername)
                {
                    if (periodsOverlap(selectedPeriod, period))
                        return false;
                }
            }
            return true;
        }
        private bool isPatientFreeAtCertainTime(Period selectedPeriod)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            foreach (var period in allPeriods)
            {
                if (period.PatientUsername == selectedPeriod.PatientUsername)
                {
                    if (periodsOverlap(selectedPeriod, period))
                        return false;
                }
            }
            return true;
        }

        private bool isRoomFreeAtCertainTime(Period selectedPeriod)
        {
            List<Period> allPeriods = _periodRepository.GetValues();
            foreach (var period in allPeriods)
            {
                if (period.RoomId == selectedPeriod.RoomId)
                {
                    if (periodsOverlap(selectedPeriod, period))
                        return false;
                }
            }
            return true;
        }


    }
}
