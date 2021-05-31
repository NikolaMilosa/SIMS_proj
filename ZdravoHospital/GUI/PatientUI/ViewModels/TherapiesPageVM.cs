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
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class TherapiesPageVM
    {
        public ScheduleAppointmentCollection Therapies { get; set; }
        public TherapyFunctions TherapyFunctions { get; set; }

        public TherapiesPageVM(SfSchedule selectedAppointment)
        {
            SetProperties();
            SetCommands();
        }

        private void SetProperties()
        {
            TherapyFunctions = new TherapyFunctions();
            SetTherapies();
        }
        private void SetTherapies()
        {
            Therapies = new ScheduleAppointmentCollection();
            foreach (var therapy in TherapyFunctions.GetPatientTherapies(PatientWindowVM.PatientUsername))
                SetDates(therapy);
        }

        private void SetDates(Therapy therapy)
        {
            foreach (var date in TherapyFunctions.GenerateDates(therapy))
                GenerateTherapyAppointment(therapy,date);
        }

        private void GenerateTherapyAppointment(Therapy therapy,DateTime date)
        {
            ScheduleAppointment appointment = new ScheduleAppointment();
            appointment.Subject = therapy.Medicine.MedicineName;
            appointment.StartTime = date;
            appointment.EndTime = date.AddMinutes(30);
            appointment.AppointmentBackground = new SolidColorBrush(Colors.Blue);
            appointment.Notes = therapy.Instructions;
            Therapies.Add(appointment);
        }

        #region Commands

        public RelayCommand TherapyCommand { get; private set; }

        #endregion

        #region CommandActions

        private void TherapyExecution(object parameter)
        {
            SfSchedule scheduler = (SfSchedule) parameter;
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowLargeOkDialog("Instruction",scheduler.SelectedAppointment.Notes);
        }

        #endregion

        #region Methods

        private void SetCommands()
        {
            TherapyCommand = new RelayCommand(TherapyExecution);
        }

     

        //private List<DateTime> GenerateTimes(Therapy therapy, string username)
        //{
        //    List<DateTime> notifications = GenerateNotificationsForEachDay(therapy);
        //    PeriodFunctions periodFunctions = new PeriodFunctions();
        //    foreach (DateTime dateTime in notifications)
        //    {
        //        ScheduleAppointment appointment = new ScheduleAppointment();
        //        appointment.Subject = therapy.Medicine.MedicineName;
        //        appointment.StartTime = dateTime;
        //        appointment.EndTime = dateTime.AddMinutes(30);
        //        appointment.AppointmentBackground = new SolidColorBrush(Colors.Blue);
        //        appointment.Notes = "In friendship diminution instrument so. Son sure";
        //        Therapies.Add(appointment);
        //        //essageBox.Show(therapy.Medicine.MedicineName + " " + dateTime.ToString());
        //    }
        //    return notifications;
        //}

     

        #endregion


    }
}
