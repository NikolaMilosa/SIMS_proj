using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for NewAppointmentPage.xaml
    /// </summary>
    public partial class NewAppointmentPage : Page
    {
        private Doctor _doctor;
        private DateTime _startTime;
        private int _duration;
        private Referral _referral;
        private Patient _patient;
        
        public Thickness TopPanelMargin { get; set; }

        public NewAppointmentPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();

            _doctor = doctor;
            _startTime = startTime;
            _duration = duration;
        }

        public NewAppointmentPage(Referral referral, Patient patient)
        {
            InitializeComponent();

            _referral = referral;
            _patient = patient;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_referral == null)
                DataContext = new NewAppointmentViewModel(NavigationService, _doctor, _startTime, _duration);
            else
                DataContext = new NewAppointmentViewModel(NavigationService, _referral, _patient);
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientsComboBox.SelectedItem as Patient;

            if (patient != null)
                NavigationService.Navigate(new PatientInfoPage(patient));
        }
    }
}
