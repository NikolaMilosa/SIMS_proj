﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Channels;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PatientWindowVM : ViewModel
    {
        #region Properties

        public string WelcomeMessage { get; private set; }
        public PatientWindow PatientWindow { get; private set; }
        public ThreadFunctions ThreadFunctions { get; private set; }
        public static string PatientUsername { get; private set; }
        private  bool _SurveyAvailable { get; set; }
        public  bool SurveyAvailable
        {
            get => _SurveyAvailable;
            set
            {
                _SurveyAvailable = value;
                OnPropertyChanged("SurveyAvailable");
            }
        }
        public static NavigationService NavigationService { get; private set; }


        #endregion

        #region Constructor

        public PatientWindowVM(string username,PatientWindow patientWindow)
        {
            SetProperties(username,patientWindow);
            CheckSurveys();
            StartThreads();
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand LogOutCommand { get; private set; }
        public RelayCommand AddAppointmentCommand { get; private set; }
        public RelayCommand NotificationCommand { get; private set; }
        public RelayCommand PeriodsCommand { get; private set; }
        public RelayCommand PeriodHistoryCommand { get; private set; }
        public RelayCommand SurveyCommand { get; private set; }
        public RelayCommand NoteCommand { get; private set; }

        #endregion

        #region CommandActions

        private void LogOutExecute(object sendes)
        {
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowYesNoDialog("Logging out","Are you sure that you want to log out?");
            if (!viewFunctions.YesPressed)
                return;
            SerializePatient();
            ThreadFunctions.KillThreads();
           CloseWindows();
        }

        private void AddAppointmentExecute(object sender)
        {
            NavigationService.Navigate(new AddAppointmentPage(null));
        }

        private void PeriodsExecute(object sender)
        {
            NavigationService.Navigate(new PeriodPage(PatientUsername));
        }

        private void NotificationsExecute(object sender)
        {
            NavigationService.Navigate(new NotificationsPage(PatientUsername));
        }

        private void PeriodHistoryExecute(object sender)
        {
            NavigationService.Navigate(new AppointmentHistoryPage(PatientUsername));
        }

        private void SurveyExecute(object sender)
        {
            NavigationService.Navigate(new SurveyPage(this));
        }

        private void NoteExecute(object sender)
        {
            NavigationService.Navigate(new NotesPage(PatientUsername));
        }

        #endregion

        #region Methods

        private void CloseWindows()
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            PatientWindow.Close();
        }
        private void SetCommands()
        {
            LogOutCommand = new RelayCommand(LogOutExecute);
            AddAppointmentCommand = new RelayCommand(AddAppointmentExecute);
            PeriodsCommand = new RelayCommand(PeriodsExecute);
            NotificationCommand = new RelayCommand(NotificationsExecute);
            PeriodHistoryCommand = new RelayCommand(PeriodHistoryExecute);
            SurveyCommand = new RelayCommand(SurveyExecute);
            NoteCommand = new RelayCommand(NoteExecute);
        }

        private void StartThreads()
        {
            ThreadFunctions = new ThreadFunctions(PatientUsername);
            ThreadFunctions.StartThreads();
        }
        private void SetProperties(string username, PatientWindow patientWindow)
        {
            PatientWindow = patientWindow;
            NavigationService = patientWindow.myFrame.NavigationService;
            PatientUsername = username;
            WelcomeMessage = "Welcome " + username;
        }
        private void CheckSurveys()
        {
            SurveyFunctions surveyFunctions = new SurveyFunctions();
            SurveyAvailable = surveyFunctions.IsSurveyAvailable(PatientUsername);
        }

        private void SerializePatient()
        {
            PatientRepository patientRepository = new PatientRepository();
            Patient patient = patientRepository.GetById(PatientUsername);
            patient.LastLogoutTime = DateTime.Now;
            patientRepository.Update(patient);
        }



        #endregion
    }
}