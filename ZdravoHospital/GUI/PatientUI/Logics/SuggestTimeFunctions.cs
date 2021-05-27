using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class SuggestTimeFunctions
    {
        public List<PeriodDTO> SuggestedPeriods { get; private set; }
        public DoctorDTO Doctor { get; private set; }
        public InjectFunctions Injection { get; private set; }

        public SuggestTimeFunctions(List<PeriodDTO> suggestedPeriods, DoctorDTO doctor)
        {
            SuggestedPeriods = suggestedPeriods;
            Doctor = doctor;
            Injection = new InjectFunctions();
            GetSuggestedPeriods();
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
            //Page.Period.DoctorUsername = ((DoctorDTO)Page.selectDoctor.SelectedItem).Username;
            //ShowSuggestedTimes();
            //Validate.ShowOkDialog("Suggested time", "Time list is updated to suggested times!");
            //Page.selectDate.SelectedDate = Page.Period.StartTime;
            //Page.selectTime.IsDropDownOpen = true;
        }

        private void ShowSuggestedTimes()
        {
            //Page.PeriodList.Clear();
            //int daysFromToday = 3;
            //while (Page.PeriodList.Count < 2)
            //{
            //    Page.PeriodList.Clear();
            //    AddFreeTimes(daysFromToday);
            //    daysFromToday++;
            //}
        }

        private void AddFreeTimes(int daysFromToday)//adding free times to timeList on the date which is passed as parameter
        {
            //List<TimeSpan> timeList = new List<TimeSpan>();
            //Validate.GenerateTimeSpan(timeList);

            //foreach (TimeSpan timeSpan in timeList) if (Page.PeriodList.Count < 4)
            //{
            //    Page.Period.StartTime = DateTime.Today.AddDays(daysFromToday);
            //    Page.Period.StartTime += timeSpan;
            //    if (PeriodFunctions.CheckPeriodAvailability(Page.Period, false))
            //        Page.PeriodList.Add(timeSpan);
            //}
        }
    }
}
