using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class SuggestTimeService
    {
        public ObservableCollection<PeriodDTO> SuggestedPeriods { get; private set; }
        public DoctorDTO Doctor { get; private set; }
        public InjectService Injection { get; private set; }
        public PeriodService PeriodFunctions { get; private set; }
        public PeriodConverter PeriodConverter { get; private set; }
        public RoomSheduleService RoomFunctions { get; private set; }

        public SuggestTimeService(ObservableCollection<PeriodDTO> suggestedPeriods, DoctorDTO doctor)
        {
            SuggestedPeriods = suggestedPeriods;
            Doctor = doctor;
            Injection = new InjectService();
            PeriodFunctions = new PeriodService();
            PeriodConverter = new PeriodConverter();
            RoomFunctions = new RoomSheduleService();
        }

        public void GetSuggestedPeriods()
        {
            int daysFromToday = 3;
            while (SuggestedPeriods.Count < 2)
            {
                SuggestedPeriods.Clear();
                AddFreeTimes(daysFromToday);
                daysFromToday++;
            }
        }


        private void AddFreeTimes(int daysFromToday)
        {
            List<TimeSpan> timeList = new List<TimeSpan>();
            Injection.GenerateTimeSpan(timeList);
            foreach (TimeSpan timeSpan in timeList) if (SuggestedPeriods.Count < 4)
            {
                Period period = GeneratePeriod(timeSpan, daysFromToday);
                    if (PeriodFunctions.CheckPeriodAvailability(period) && period.RoomId!=-1)
                        SuggestedPeriods.Add(PeriodConverter.GetPeriodDTO(period));
                }
        }

        private Period GeneratePeriod(TimeSpan timeSpan,int daysFromToday)
        {
            Period period = new Period(DateTime.Today.AddDays(daysFromToday), 30, PeriodType.APPOINTMENT,
                PatientWindowVM.PatientUsername, Doctor.Username, false, PeriodFunctions.GeneratePeriodId());
            period.StartTime += timeSpan;
            period.RoomId = RoomFunctions.GetFreeRoom(period);
            return period;
        }
    }
}
