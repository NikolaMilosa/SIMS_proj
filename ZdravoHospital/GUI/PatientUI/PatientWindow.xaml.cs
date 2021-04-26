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
    public partial class PatientWindow : Window,INotifyPropertyChanged
    {
        public string WelcomeMessage { get; set; }

        public Patient Patient { get; set; }

        private bool _SurveyAvailable { get; set; }
        public  bool SurveyAvailable {
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

        public PatientWindow(string username)
        {
            InitializeComponent();
            DataContext = this;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            WelcomeMessage = "Welcome " + username;
            Model.Resources.OpenPatients();
            Patient = Model.Resources.patients[username];
            SurveyAvailable = Validate.IsSurveyAvailable(username);
            //obrisi-----------------------------------------------------
            //Prescription prescription = new Prescription();
            //prescription.TherapyList = new List<Therapy>();
            //Medicine medicine = new Medicine("Brufen");
            //Therapy therapy = new Therapy();
            //therapy.StartHours = DateTime.Now.AddMinutes(2);//DateTime.Today.AddHours(8);
            //therapy.EndDate = therapy.StartHours.AddDays(4);
            //therapy.Medicine = medicine;
            //therapy.TimesPerDay = 2;
            //therapy.PauseInDays = 1;
            //prescription.TherapyList.Add(therapy);
            //Patient.Prescriptions = new List<Prescription>();
            //Patient.Prescriptions.Add(prescription); 
            //----------------------------------------------------------------------------
            Thread thread = new Thread(new ParameterizedThreadStart(Validate.therapyNotification));
            
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(username);

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected  void OnPropertyChanged(string name)
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
            myFrame.Navigate(new AddAppointmentPage(null,true,Patient.Username));
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
