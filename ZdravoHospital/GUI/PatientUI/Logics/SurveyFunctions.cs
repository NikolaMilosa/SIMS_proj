using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class SurveyFunctions
    {
        public SurveyFunctions()
        {

        }

        public  bool IsSurveyAvailable(string username)
        {
            bool availability = false;
            int numOfPeriods = GetCompletedPeriodsNum(username);
            if (numOfPeriods >= 3 && !AnyRecentSurveys(username))
                availability = true;
            return availability;
        }

        private  int GetCompletedPeriodsNum(string username)
        {
            int periodNum = 0;
            PeriodRepository periodRepository = new PeriodRepository();
            foreach (Period period in periodRepository.GetValues())
            {
                if (period.PatientUsername.Equals(username) && period.HasPassed())
                {
                    periodNum++;
                }
            }
            return periodNum;
        }

        private  bool AnyRecentSurveys(string username)
        {
            bool recentSurvey = false;
            SurveyRepository surveyRepository = new SurveyRepository();
            List<Survey> surveys = surveyRepository.GetValues();
            foreach (Survey survey in surveys)
            {
                if (survey.PatientUsername.Equals(username) && survey.IsWithin2WeeksFromNow())
                {
                    recentSurvey = true;
                    break;
                }
            }
            return recentSurvey;
        }
    }
}
