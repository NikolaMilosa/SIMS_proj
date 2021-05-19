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
           
            //Patient = Model.Resources.patients[username];
            PatientRepository patientRepository = new PatientRepository();
            Patient=patientRepository.GetById(username);
        }

        public void SetWindowParameters(string username)
        {
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WelcomeMessage = "Welcome " + username;
            myFrame.Navigate(new AppointmentPage(username));
        }

        public void StartThreads()
        {
            StartNotificationThread();
            StartTrollThread();
            StartNoteThread();
        }

        private void StartNotificationThread()
        {
            Thread notificationThread= new Thread(new ParameterizedThreadStart(Validate.TherapyNotification));
            notificationThread.SetApartmentState(ApartmentState.STA);
            notificationThread.Start(Patient.Username);
        }

        private void StartNoteThread()
        {
            Thread notificationNoteThread = new Thread(new ParameterizedThreadStart(Validate.NoteNotification));
            notificationNoteThread.SetApartmentState(ApartmentState.STA);
            notificationNoteThread.Start(Patient.Username);
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

        private void LogOutButton_Click(object sender, RoutedEventArgs e)
        {
            LogOutDialog logOutDialog = new LogOutDialog(this);
            logOutDialog.ShowDialog();
        }

        private void AddAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AddAppointmentPage(null, true, Patient.Username));
        }

        private void AppointmentsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AppointmentPage(Patient.Username));
        }

        private void NotificationsButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new NotificationsPage(Patient.Username));
        }

        private void AppointmentHistoryButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new AppointmentHistoryPage(Patient.Username));
        }

        private void SurveyButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new SurveyPage(this));
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            myFrame.Navigate(new NotesPage(Patient.Username));
        }
    }
}
