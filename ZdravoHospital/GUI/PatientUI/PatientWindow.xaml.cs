using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for PatientWindow.xaml
    /// </summary>
    public partial class PatientWindow : Window, INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        public string WelcomeMessage { get; set; }

        public  string PatientUsername { get; set; }

        public static Frame Frame { get; private set; }

        private bool _SurveyAvailable { get; set; }
        public bool SurveyAvailable {
            get
            {
                return _SurveyAvailable;
            }
            set
            {
                _SurveyAvailable = value;
                OnPropertyChanged("SurveyAvailable");
            }
        }

        private static int recentActions;

        public static int RecentActionsNum
        {
            get
            {
                return recentActions;
            }
            set
            {
                recentActions = value;
                if(recentActions==5)
                    Validate.ShowOkDialog("Warning","Account blocked! Contact our support!");
            }
        }

        public PatientWindow(string username)
        {
            InitializeComponent();
            SetProperties(username);
            SetWindowParameters();
            CheckSurveys();
            StartThreads();
        }

        public void CheckSurveys()
        {
            SurveyFunctions surveyFunctions = new SurveyFunctions();
            SurveyAvailable = surveyFunctions.IsSurveyAvailable(PatientUsername);
        }

        private void SetProperties(string username)
        {
            PatientUsername = username;
            WelcomeMessage = "Welcome " + username;
            Frame = myFrame;
        }

        public void SetWindowParameters()
        {
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            //myFrame.Navigate(new PeriodPage(PatientUsername));
        }

        public void StartThreads()
        {
            StartNotificationThread();
            StartTrollThread();
            StartNoteThread();
        }

        private void StartNotificationThread()
        {
            Thread notificationThread= new Thread(new ParameterizedThreadStart(ThreadTherapyFunctions.TherapyNotification));
            notificationThread.SetApartmentState(ApartmentState.STA);
            notificationThread.Start(PatientUsername);
        }

        private void StartNoteThread()
        {
            Thread notificationNoteThread = new Thread(new ParameterizedThreadStart(ThreadNoteFunctions.NoteNotification));
            notificationNoteThread.SetApartmentState(ApartmentState.STA);
            notificationNoteThread.Start(PatientUsername);
        }

        private void StartTrollThread()
        {
            Thread trollThread = new Thread(new ParameterizedThreadStart(ThreadTrollFunctions.ResetActionsNum));
            trollThread.SetApartmentState(ApartmentState.STA);
            trollThread.Start(PatientUsername);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOutDialog logOutDialog = new LogOutDialog(this);
            logOutDialog.ShowDialog();
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AddAppointmentPage(null, true, PatientUsername));
        }

        private void AppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new PeriodPage(PatientUsername));
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new NotificationsPage(PatientUsername));
        }

        private void AppointmentHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AppointmentHistoryPage(PatientUsername));
        }

        private void SurveyButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new SurveyPage(this));
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new NotesPage(PatientUsername));
        }
    }
}
