using Model;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for ReferralPage.xaml
    /// </summary>
    public partial class ReferralPage : Page
    {
        private Referral _referral;
        private Doctor _referringDoctor;
        private Patient _patient;
        private Period _period;

        public ReferralPage(Doctor referringDoctor, Patient patient, Period period)
        {
            InitializeComponent();

            _referringDoctor = referringDoctor;
            _patient = patient;
            _period = period;
        }

        public ReferralPage(Referral referral, Patient patient)
        {
            InitializeComponent();

            _referral = referral;
            _patient = patient;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (_referral == null)
                DataContext = new ReferralViewModel(NavigationService, _referringDoctor, _patient, _period);
            else
                DataContext = new ReferralViewModel(NavigationService, _referral, _patient);
        }

        private void ReferredAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new AppointmentPage(Referral.Period));
        }

        private void ReferredOperationButton_Click(object sender, RoutedEventArgs e)
        {
            //bool isReadonlyModeOn = !Referral.ReferringDoctorUsername.Equals(App.currentUser);
            //NavigationService.Navigate(new OperationPage(Referral.Period, isReadonlyModeOn));
        }

        private void DoctorsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (UseStackPanel.Visibility != Visibility.Visible)
                return;

            if ((DoctorsComboBox.SelectedItem as Doctor).SpecialistType.SpecializationName.Equals("Doctor"))
                UseReferralOperationButton.Visibility = Visibility.Collapsed;
            else
                UseReferralOperationButton.Visibility = Visibility.Visible;
        }
    }
}
