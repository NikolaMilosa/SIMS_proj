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

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            DataContext = new ReferralViewModel(NavigationService, _referringDoctor, _patient, _period);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            //UseStackPanel.Visibility = Visibility.Collapsed;
            //ConfirmButton.Visibility = Visibility.Visible;
            //EditButton.Visibility = Visibility.Collapsed;
            //DoctorsComboBox.IsHitTestVisible = true;
            //DoctorsComboBox.IsTabStop  = true;
            //NoteTextBox.IsReadOnly = false;
            //DaysToUseTextBox.IsReadOnly = false;
        }

        private void UseReferralAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new NewAppointmentPage(Referral, Patient));
        }

        private void UseReferralOperationButton_Click(object sender, RoutedEventArgs e)
        {
            //NavigationService.Navigate(new NewOperationPage(Referral, Patient));
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
