using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;
using Model;
using Syncfusion.UI.Xaml.Schedule;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class TherapyFunctions
    {
        public PeriodFunctions PeriodFunctions { get; set; }

        public TherapyFunctions()
        {
            PeriodFunctions = new PeriodFunctions();
        }

        public List<Therapy> GetPatientTherapies(string username)
        {
            return PeriodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(username) && period.Prescription != null).SelectMany(period => period.Prescription.TherapyList).ToList();
        }

        public List<DateTime> GenerateDates(Therapy therapy)
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
