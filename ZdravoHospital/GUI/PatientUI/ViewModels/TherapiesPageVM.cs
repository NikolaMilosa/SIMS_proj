using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Media;
using Model;
using Syncfusion.UI.Xaml.Schedule;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class TherapiesPageVM
    {
        public ScheduleAppointmentCollection Therapies { get;  set; } 
        public TherapiesPageVM()
        {
            SetTherapies();
            TherapyNotification(PatientWindowVM.PatientUsername);
        }

        private void SetTherapies()
        {
            Therapies = new ScheduleAppointmentCollection();

        }

        public  void TherapyNotification(object patientUsername)
        {
            string username = (string)patientUsername;

            PeriodFunctions periodFunctions = new PeriodFunctions();
            
                foreach (var period in periodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(username) && period.Prescription != null))
                {
                    GeneratePrescriptionTimes(period.Prescription, username);
                }

             
        }

        private  void GeneratePrescriptionTimes(Prescription prescription, string username)
        {
            foreach (Therapy therapy in prescription.TherapyList)
                GenerateTimes(therapy, username);

        }

        private  List<DateTime> GenerateTimes(Therapy therapy, string username)
        {
            List<DateTime> notifications = GenerateNotificationsForEachDay(therapy);
            PeriodFunctions periodFunctions = new PeriodFunctions();
            foreach (DateTime dateTime in notifications)
            {
                ScheduleAppointment appointment = new ScheduleAppointment();
                appointment.Subject = therapy.Medicine.MedicineName;
                appointment.StartTime = dateTime;
                appointment.EndTime = dateTime.AddMinutes(30);
                appointment.AppointmentBackground = new SolidColorBrush(Colors.Blue);
                appointment.Notes = "Blablabla";
                Therapies.Add(appointment);
                //essageBox.Show(therapy.Medicine.MedicineName + " " + dateTime.ToString());
            }
            return notifications;
        }

        private  List<DateTime> GenerateNotificationsForEachDay(Therapy therapy)
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
