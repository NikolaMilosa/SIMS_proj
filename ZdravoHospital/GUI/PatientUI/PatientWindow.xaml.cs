using Model;
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

        public Patient Patient { get; set; }

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

        public static int RecentActionsNum { get; set; }

        public PatientWindow(string username)
        {
            InitializeComponent();
            SetWindowParameters(username);
            LoadPatient(username);
            CheckSurveys(username);
            StartThreads();
        }

        public void CheckSurveys(string username)
        {
            SurveyAvailable = Validate.IsSurveyAvailable(username);
        }

        public void LoadPatient(string username)
        {
            Model.Resources.OpenPatients();
            Patient = Model.Resources.patients[username];
        }

        public void SetWindowParameters(string username)
        {
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WelcomeMessage = "Welcome " + username;
        }

        public void StartThreads()
        {
            StartNotificationThread();
            StartTrollThread();
        }

        private void StartNotificationThread()
        {
            Thread notificationThread= new Thread(new ParameterizedThreadStart(Validate.TherapyNotification));
            notificationThread.SetApartmentState(ApartmentState.STA);
            notificationThread.Start(Patient.Username);
        }

        private void StartTrollThread()
        {
            Thread trollThread = new Thread(new ParameterizedThreadStart(Validate.ResetActionsNum));
            trollThread.SetApartmentState(ApartmentState.STA);
            trollThread.Start(Patient);
        }

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void logOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOutDialog logOutDialog = new LogOutDialog(this);
            logOutDialog.ShowDialog();
        }

        private void addAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AddAppointmentPage(null, true, Patient.Username));
        }

        private void appointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AppointmentPage(Patient.Username));
        }

        private void notificationsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new NotificationsPage(Patient.Username));
        }

        private void appointmentHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AppointmentHistoryPage(Patient.Username));
        }

        private void surveyButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new SurveyPage(this));
        }

    }
}
