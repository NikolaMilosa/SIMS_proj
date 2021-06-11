using Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Repository.SurveyPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class SurveyFunctions
    {
        public SurveyRepository SurveyRepository { get; private set; }
        public SurveyFunctions()
        {
            SurveyRepository = new SurveyRepository();
        }

        public List<Survey> GetAll() => SurveyRepository.GetValues();

        public void SerializeSurvey(Survey survey)
        {
            SurveyRepository.Create(survey);
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
            PeriodService periodFunctions = new PeriodService();
            return periodFunctions.GetAllPeriods().Count(period => period.PatientUsername.Equals(username) && period.HasPassed());
        }

        private  bool AnyRecentSurveys(string username)
        {
            List<Survey> surveys = SurveyRepository.GetValues();
            return surveys.Any(survey => survey.PatientUsername.Equals(username) && survey.IsWithin2WeeksFromNow());
        }
    }
}
