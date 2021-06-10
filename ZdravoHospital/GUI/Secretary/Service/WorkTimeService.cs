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
        

        public Shift GetDoctorShiftByDate(Doctor doctor, DateTime date)
        {
            if (isDateInVacationTime(doctor, date))
                return Shift.FREE;

            DoctorsShift singleDayShift = tryFindSingleDayShift(doctor, date);
            if (singleDayShift != null)
                return singleDayShift.ScheduledShift;

            return tryFindRegularShift(doctor, date);

        }

        private bool isDateInsideInterval(DateTime date, DateTime startTime, int duration)
        {
            if (date.Date >= startTime.Date && date.Date <= startTime.AddDays(duration).Date)
                return true;
            else
                return false;
        }

        private bool isDateInVacationTime(Doctor doctor, DateTime date)
        {
            foreach (var vacation in doctor.ShiftRule.Vacations)
            {
                if (isDateInsideInterval(date, vacation.VacationStartTime, vacation.NumberOfFreeDays))
                    return true;
            }
            return false;
        }

        private DoctorsShift tryFindSingleDayShift(Doctor doctor, DateTime date)
        {
            foreach(var shift in doctor.ShiftRule.SingleDayShifts)
            {
                if (shift.ShiftStart.Date == date.Date)
                    return shift;
            }
            return null;
        }

        private Shift tryFindRegularShift(Doctor doctor, DateTime date)
        {
            if(doctor.ShiftRule.RegularShift == null)
                return Shift.FREE;

            if (date.Date < doctor.ShiftRule.RegularShift.ShiftStart.Date)
                return Shift.FREE;

            if (doctor.ShiftRule.RegularShift.ShiftStart.Date == date)
                return doctor.ShiftRule.RegularShift.ScheduledShift;

            int dateDifference = (int)(Math.Abs((doctor.ShiftRule.RegularShift.ShiftStart.Date - date).TotalDays));
            return (Shift)((int)(doctor.ShiftRule.RegularShift.ScheduledShift + dateDifference) % 4);

        }

        public DateTime getDoctorsShiftStartTime(Doctor doctor, DateTime date)
        {
            Shift shift = GetDoctorShiftByDate(doctor, date);
            if (shift == Shift.FIRST)
                return new DateTime(date.Year, date.Month, date.Day, 6, 0, 0);
            else if (shift == Shift.SECOND)
                return new DateTime(date.Year, date.Month, date.Day, 14, 0, 0);
            else if (shift == Shift.THIRD)
                return new DateTime(date.Year, date.Month, date.Day, 22, 0, 0);
            else
                return DateTime.MinValue;
        }
    }
}
