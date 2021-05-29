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
    public class SuggestTimeFunctions
    {
        public ObservableCollection<PeriodDTO> SuggestedPeriods { get; private set; }
        public DoctorDTO Doctor { get; private set; }
        public InjectFunctions Injection { get; private set; }
        public PeriodFunctions PeriodFunctions { get; private set; }
        public PeriodConverter PeriodConverter { get; private set; }
        public RoomSheduleFunctions RoomFunctions { get; private set; }

        public SuggestTimeFunctions(ObservableCollection<PeriodDTO> suggestedPeriods, DoctorDTO doctor)
        {
            SuggestedPeriods = suggestedPeriods;
            Doctor = doctor;
            Injection = new InjectFunctions();
            PeriodFunctions = new PeriodFunctions();
            PeriodConverter = new PeriodConverter();
            RoomFunctions = new RoomSheduleFunctions();
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
