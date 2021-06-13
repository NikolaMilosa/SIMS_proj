using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.Strategy
{
    public class SuggestDate : SuggestAbstract, ISuggestStrategy
    {
        public string DoctorsUsername { get; private set; }
        public SuggestDate(ObservableCollection<PeriodDTO> suggestedPeriods,string doctorUsername):base(suggestedPeriods)
        {
            DoctorsUsername = doctorUsername;
        }
        public void Suggest()
        {
            GetSuggestedPeriods();
        }

        private void GetSuggestedPeriods()
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
                    if (PeriodService.CheckPeriodAvailability(period) && period.RoomId != -1)
                        SuggestedPeriods.Add(PeriodConverter.GetPeriodDTO(period));
                }
        }

        private Period GeneratePeriod(TimeSpan timeSpan, int daysFromToday)
        {
            Period period = new Period(DateTime.Today.AddDays(daysFromToday), 30, PeriodType.APPOINTMENT,
                PatientWindowVM.PatientUsername, DoctorsUsername, false, PeriodService.GeneratePeriodId());
            period.StartTime += timeSpan;
       
            period.RoomId = RoomScheduleService.GetFreeRoom(period);
            return period;
        }

    }
}
