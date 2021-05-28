using Model;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for NewOperationPage.xaml
    /// </summary>
    public partial class NewOperationPage : Page
    {
        private Doctor _doctor;
        private DateTime _startTime;
        private int _duration;
        private Referral _referral;
        private Patient _patient;

        public Thickness TopPanelMargin { get; set; }

        public NewOperationPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();

            _doctor = doctor;
            _startTime = startTime;
            _duration = duration;
        }

        public NewOperationPage(Referral referral, Patient patient)
        {
            InitializeComponent();

            _referral = referral;
            _patient = patient;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_referral == null)
                DataContext = new NewOperationViewModel(NavigationService, _doctor, _startTime, _duration);
            else
                DataContext = new NewOperationViewModel(NavigationService, _referral, _patient);
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientsComboBox.SelectedItem as Patient;

            if (patient != null)
                NavigationService.Navigate(new PatientInfoPage(patient));
        }
    }
}
