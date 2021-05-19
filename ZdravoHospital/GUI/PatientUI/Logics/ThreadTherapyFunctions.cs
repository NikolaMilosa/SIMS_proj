using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadTherapyFunctions
    {
        public static void TherapyNotification(object patientUsername)
        {
            string username = (string)patientUsername;
            
            PeriodRepository periodRepository = new PeriodRepository();
            while (true)
            {
                foreach (Period period in periodRepository.GetValues())
                {
                    if (period.PatientUsername.Equals(username) && period.Prescription != null)
                    {
                        GeneratePrescriptionTimes(period.Prescription, username);
                    }
                }
                Validate.SleepForGivenMinutes(5);
            }
        }

        private static void GeneratePrescriptionTimes(Prescription prescription,string username)
        {
            foreach (Therapy therapy in prescription.TherapyList)
                GenerateTimes(therapy, username);

        }

        private static List<DateTime> GenerateTimes(Therapy therapy,string username)
        {
            List<DateTime> notifications = GenerateNotificationsForEachDay(therapy);
            PeriodFunctions periodFunctions = new PeriodFunctions(username);
            foreach (DateTime dateTime in notifications)
                if (periodFunctions.IsPeriodWithinGivenMinutes(dateTime, 5))
                    Validate.ShowOkDialog("Therapy", "You have prescripted " + therapy.Medicine.MedicineName + " at " + dateTime.ToString("HH:mm"));

            return notifications;
        }

        private static List<DateTime> GenerateNotificationsForEachDay(Therapy therapy)
        {
            List<DateTime> notifications = new List<DateTime>();
            DateTime dateIterator = therapy.StartHours;
            while (dateIterator.Date < therapy.EndDate.Date)
            {
                for (int i = 0; i < therapy.TimesPerDay; ++i)
                    notifications.Add(dateIterator.AddHours(i * 24 / therapy.TimesPerDay));

                dateIterator = dateIterator.AddDays(therapy.PauseInDays + 1);
            }
            return notifications;
        }


    }
}
