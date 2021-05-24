using Model;
using Repository.DoctorPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.DTOs;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class WorkTimeService
    {
        private IDoctorRepository _doctorRepository;
        public WorkTimeService()
        {
            _doctorRepository = new DoctorRepository();
        }
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetValues();
        }
        public void SetDoctorWorkSchedule(DoctorWorkDTO doctorWorkDTO)
        {
            setShiftRule(doctorWorkDTO);
            //_doctorRepository.Update(doctorWorkDTO.Doctor);
            ProcessDoctorsShiftRule(doctorWorkDTO.Doctor);
        }

        public void SetDoctorHolidaySchedule(DoctorWorkDTO doctorWorkDTO)
        {
            doctorWorkDTO.Doctor.ShiftRule.VacationStartTime = doctorWorkDTO.VacationStart;
            doctorWorkDTO.Doctor.ShiftRule.NumberOfFreeDays = doctorWorkDTO.NumberOfVacationDays;
            //_doctorRepository.Update(doctorWorkDTO.Doctor);
            ProcessDoctorsShiftRule(doctorWorkDTO.Doctor);
        }
        private void setShiftRule(DoctorWorkDTO doctorWorkDTO)
        {
            doctorWorkDTO.Doctor.ShiftRule.ScheduledShift = doctorWorkDTO.SelectedShift;
            doctorWorkDTO.Doctor.ShiftRule.ShiftStart = doctorWorkDTO.ShiftStart;
            if(doctorWorkDTO.NumberOfVacationDays != 0)
            {
                doctorWorkDTO.Doctor.ShiftRule.VacationStartTime = doctorWorkDTO.VacationStart;
                doctorWorkDTO.Doctor.ShiftRule.NumberOfFreeDays = doctorWorkDTO.NumberOfVacationDays;
            }
        }

        public void ProcessDoctorsShiftRule(Doctor doctor)
        {
            /*if (isDateDayOff(doctor, DateTime.Now)) 
            {
                doctor.ShiftRule.CurrentShift = Shift.FREE;
                _doctorRepository.Update(doctor);
                return;
            }*/

            if(doctor.ShiftRule.ShiftStart.Date == DateTime.Now.Date)
            {
                doctor.ShiftRule.CurrentShift = doctor.ShiftRule.ScheduledShift;
            }
            else if (doctor.ShiftRule.ShiftStart.Date < DateTime.Now.Date)
            {
                int dateDifference = (int)(Math.Abs((doctor.ShiftRule.ShiftStart.Date - DateTime.Now.Date).TotalDays));
                doctor.ShiftRule.CurrentShift = (Shift)((int)(doctor.ShiftRule.ScheduledShift + dateDifference) % 4);
            }

            _doctorRepository.Update(doctor);
        }

        private bool isDateDayOff(Doctor doctor, DateTime date)
        {
            DateTime vacationEndTime = doctor.ShiftRule.VacationStartTime.AddDays(doctor.ShiftRule.NumberOfFreeDays);
            if (date.Date >= doctor.ShiftRule.VacationStartTime.Date && date.Date < vacationEndTime.Date)
            {
                return true;
            }
            return false;
        }

        public Shift GetDoctorShiftByDate(Doctor doctor, DateTime date)
        {
            if (date.Date < doctor.ShiftRule.ShiftStart.Date)
                return Shift.FREE;

            if (doctor.ShiftRule.ShiftStart.Date == date)
            {
                return doctor.ShiftRule.ScheduledShift;
            }

            if (isDateDayOff(doctor, date))
                return Shift.FREE;

            int dateDifference = (int)(Math.Abs((doctor.ShiftRule.ShiftStart.Date - date).TotalDays));
            return (Shift)((int)(doctor.ShiftRule.ScheduledShift + dateDifference) % 4);
        }
    }
}
