using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Windows.Navigation;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PatientWindowVM : ViewModel
    {
        #region Properties

        public string WelcomeMessage { get; private set; }
        public PatientWindow PatientWindow { get; private set; }
        public string PatientUsername { get; private set; }
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
        public NavigationService NavigatonService { get; private set; }


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
            LogOutDialog logOutDialog = new LogOutDialog(PatientWindow, PatientUsername);
            logOutDialog.ShowDialog();
        }

        private void AddAppointmentExecute(object sender)
        {
            NavigatonService.Navigate(new AddAppointmentPage(null, true, PatientUsername));
        }

        private void PeriodsExecute(object sender)
        {
            NavigatonService.Navigate(new PeriodPage(PatientUsername));
        }

        private void NotificationsExecute(object sender)
        {
            NavigatonService.Navigate(new NotificationsPage(PatientUsername));
        }

        private void PeriodHistoryExecute(object sender)
        {
            NavigatonService.Navigate(new AppointmentHistoryPage(PatientUsername));
        }

        private void SurveyExecute(object sender)
        {
            NavigatonService.Navigate(new SurveyPage(this));
        }

        private void NoteExecute(object sender)
        {
            NavigatonService.Navigate(new NotesPage(PatientUsername));
        }

        #endregion

        #region Methods

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
            ThreadFunctions threadFunctions = new ThreadFunctions(PatientUsername);
            threadFunctions.StartThreads();
        }
        private void SetProperties(string username, PatientWindow patientWindow)
        {
            PatientWindow = patientWindow;
            NavigatonService = patientWindow.myFrame.NavigationService;
            PatientUsername = username;
            WelcomeMessage = "Welcome " + username;
        }
        private void CheckSurveys()
        {
            SurveyFunctions surveyFunctions = new SurveyFunctions();
            SurveyAvailable = surveyFunctions.IsSurveyAvailable(PatientUsername);
        }




        #endregion
    }
}
