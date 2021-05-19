using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModel;
using System.Threading;
using Model.Repository;

namespace ZdravoHospital.GUI.PatientUI.Validations
{
    public static class Validate
    {
        public static Patient LoadPatient(string username)
        {
            PatientRepository patientRepository = new PatientRepository();
            return patientRepository.GetById(username);
        }
        public static void ShowOkDialog(string title, string content)
        {
            CustomOkDialog customOkDialog = new CustomOkDialog(title, content);
            customOkDialog.ShowDialog();
        }
        public static bool TrollDetected()
        {
            bool detected = false;
            if (PatientWindow.RecentActionsNum >= 5)
            {
                detected = true;
                ShowOkDialog("Troll detected", "Too much recent actions have been detected! Please contact our support!");
            }
            return detected;
        }
        public static bool IsSurveyAvailable(string username)
        {
            bool availability = false;
            //Resources.OpenPeriods();
            int numOfPeriods = GetCompletedPeriodsNum(username);
            if (numOfPeriods >= 3 && !AnyRecentSurveys(username))
                availability = true;
            return availability;
        }

        public static int GetCompletedPeriodsNum(string username)
        {
            int periodNum = 0;
            PeriodRepository periodRepository = new PeriodRepository();
            //foreach (Period period in Model.Resources.periods)
            foreach (Period period in periodRepository.GetValues())
            {
                if (period.PatientUsername.Equals(username) && period.HasPassed())
                {
                    periodNum++;
                }
            }
            return periodNum;
        }

        public static bool AnyRecentSurveys(string username)
        {
            bool recentSurvey = false;
            Model.Resources.OpenSurveys();
            foreach (Survey survey in Resources.surveys)
            {
                if (survey.PatientUsername.Equals(username) && survey.IsWithin2WeeksFromNow())
                {
                    recentSurvey = true;
                    break;
                }
            }
            return recentSurvey;
        }
        public static void SleepForGivenMinutes(int minutes)
        {
            Thread.Sleep(TimeSpan.FromMinutes(minutes));
        }

        public static bool IsPeriodWithinGivenMinutes(DateTime dateTime,int minutes)
        {
            bool itIs = false;
            if (dateTime >= DateTime.Now && dateTime <= DateTime.Now.AddMinutes(minutes))
                itIs = true;

            return itIs;
        }

        public static void GenerateTimeSpan(List<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }

        public static void GenerateObesrvableTimes(ObservableCollection<TimeSpan> timeList)
        {
            timeList.Add(new TimeSpan(8, 0, 0));
            timeList.Add(new TimeSpan(8, 30, 0));
            timeList.Add(new TimeSpan(9, 0, 0));
            timeList.Add(new TimeSpan(9, 30, 0));
            timeList.Add(new TimeSpan(10, 0, 0));
            timeList.Add(new TimeSpan(10, 30, 0));
            timeList.Add(new TimeSpan(11, 0, 0));
            timeList.Add(new TimeSpan(11, 30, 0));
            timeList.Add(new TimeSpan(12, 0, 0));
            timeList.Add(new TimeSpan(12, 30, 0));
            timeList.Add(new TimeSpan(13, 0, 0));
            timeList.Add(new TimeSpan(13, 30, 0));
            timeList.Add(new TimeSpan(14, 0, 0));
            timeList.Add(new TimeSpan(14, 30, 0));
            timeList.Add(new TimeSpan(15, 0, 0));
            timeList.Add(new TimeSpan(15, 30, 0));
        }
    }
}
