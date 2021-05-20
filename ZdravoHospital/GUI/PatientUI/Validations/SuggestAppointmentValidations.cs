using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.Validations
{

    public class SuggestAppointmentValidations
    {
        public AddAppointmentPage Page { get; set; }

        public PeriodFunctions PeriodFunctions { get; set; }
        public SuggestDoctorFunctions DoctorFunctions { get; set; }
        public SuggestAppointmentValidations(AddAppointmentPage addAppointmentPage)
        {
            Page = addAppointmentPage;
            PeriodFunctions = new PeriodFunctions(Page.Period.PatientUsername);
            DoctorFunctions = new SuggestDoctorFunctions(Page.Period.PatientUsername);
        }

        public bool IsOnlyTimeSelected()
        {
            bool yes = false;
            if (Page.selectDoctor.SelectedItem == null && Page.selectDate.SelectedDate != null && Page.selectTime.SelectedItem != null)
                yes = true;

            return yes;
        }

        public bool IsOnlyDoctorSelected()
        {
            bool yes = false;
            if (Page.selectDoctor.SelectedItem != null && Page.selectDate.SelectedDate == null && Page.selectTime.SelectedItem == null)
                yes = true;

            return yes;
        }

        public void SuggestTime()
        {
            Page.Period.DoctorUsername = ((DoctorDTO)Page.selectDoctor.SelectedItem).Username;
            ShowSuggestedTimes();
            Validate.ShowOkDialog("Suggested time", "Time list is updated to suggested times!");
            Page.selectDate.SelectedDate = Page.Period.StartTime;
            Page.selectTime.IsDropDownOpen = true;
        }

        public void ShowSuggestedTimes()
        {
            Page.PeriodList.Clear();
            int daysFromToday = 3;
            while (Page.PeriodList.Count < 2)
            {
                Page.PeriodList.Clear();
                AddFreeTimes(daysFromToday);
                daysFromToday++;
            }
        }

        public void AddFreeTimes(int daysFromToday)//adding free times to timeList on the date which is passed as parameter
        {
            List<TimeSpan> timeList = new List<TimeSpan>();
            Validate.GenerateTimeSpan(timeList);

            foreach (TimeSpan timeSpan in timeList) if (Page.PeriodList.Count < 4)
                {
                    Page.Period.StartTime = DateTime.Today.AddDays(daysFromToday);
                    Page.Period.StartTime += timeSpan;
                    if (PeriodFunctions.CheckPeriodAvailability(Page.Period, false))
                        Page.PeriodList.Add(timeSpan);
                }
        }

        public void SuggestDoctors()
        {
            Page.Period.StartTime = (DateTime)Page.selectDate.SelectedDate;
            Page.Period.StartTime += (TimeSpan)Page.selectTime.SelectedItem;
            if (!PeriodFunctions.CheckPeriodAvailability(Page.Period, true))
                return;

            AddDoctorsToList();
        }

        public void AddDoctorsToList()//if they are free at selected time
        {
            DoctorFunctions.SuggestDoctor(Page.Period, Page.DoctorList);
            if (Page.DoctorList.Count == 0)
                Validate.ShowOkDialog("Warning", "There is no available doctor at the selected time!");
            else
            {
                Validate.ShowOkDialog("Suggested doctor", "Doctor list is updated to suggested doctors!");
                Page.selectDoctor.IsDropDownOpen = true;
            }
        }

    
    }
}
