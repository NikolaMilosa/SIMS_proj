using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class DoctorWorkDTO
    {
        public Doctor Doctor { get; set; }
        public Shift SelectedShift { get; set; }
        public DateTime ShiftStart { get; set; }
        public DateTime VacationStart { get; set; }
        public int NumberOfVacationDays { get; set; }

        public DoctorWorkDTO(Doctor doctor, Shift selectedShift, DateTime shiftStart, DateTime vacationStart, int numberOfDays)
        {
            Doctor = doctor;
            SelectedShift = selectedShift;
            ShiftStart = shiftStart;
            VacationStart = vacationStart;
            NumberOfVacationDays = numberOfDays;
        }
    }
}
