using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class DoctorShiftsViewDTO
    {
        public WorkTimeService WorkTimeService { get; set; }
        public Doctor Doctor { get; set; }
        public Shift TodayShift { get; set; }
        public Shift TomorrowShift { get; set; }
        public Shift DayAfterTomorrowShift { get; set; }
        public DoctorShiftsViewDTO(Doctor doctor)
        {
            Doctor = doctor;
            WorkTimeService = new WorkTimeService();
            TodayShift = WorkTimeService.GetDoctorShiftByDate(Doctor, DateTime.Today);
            TomorrowShift = WorkTimeService.GetDoctorShiftByDate(Doctor, DateTime.Today.AddDays(1));
            DayAfterTomorrowShift = WorkTimeService.GetDoctorShiftByDate(Doctor, DateTime.Today.AddDays(2));
        }
    }
}
