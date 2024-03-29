﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
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
        public TherapyService TherapyFunctions { get; set; }

        public TherapiesPageVM(SfSchedule selectedAppointment)
        {
            SetProperties();
            SetCommands();
        }

      
        #region Commands

        public RelayCommand TherapyCommand { get; private set; }
        public RelayCommand GeneratePdfCommnand { get; private set; }

        #endregion

        #region CommandActions

        private void TherapyExecution(object parameter)
        {
            SfSchedule scheduler = (SfSchedule) parameter;
            ViewService viewFunctions = new ViewService();
            viewFunctions.ShowTherapyDialog("Instruction",scheduler.SelectedAppointment.Notes);
        }

        private void GeneratePdfExecution(object parameter)
        {
            PrintDialog printDialog = new PrintDialog();
            Grid grid = (Grid) parameter;
            grid.Height = 2550;
            if (printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual((Grid)parameter, "Invoice");
            }
            grid.Height = 528;
        }

        #endregion

        #region Methods

        private void SetCommands()
        {
            TherapyCommand = new RelayCommand(TherapyExecution);
            GeneratePdfCommnand = new RelayCommand(GeneratePdfExecution);
        }

        private void SetProperties()
        {
            TherapyFunctions = new TherapyService();
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
                GenerateTherapyAppointment(therapy, date);
        }

        private void GenerateTherapyAppointment(Therapy therapy, DateTime date)
        {
            ScheduleAppointment appointment = new ScheduleAppointment
            {
                Subject = therapy.Medicine.MedicineName,
                StartTime = date,
                EndTime = date.AddMinutes(30),
                AppointmentBackground = new SolidColorBrush(Colors.Blue),
                Notes = therapy.Instructions
            };
            Therapies.Add(appointment);
        }

        #endregion


    }
}
