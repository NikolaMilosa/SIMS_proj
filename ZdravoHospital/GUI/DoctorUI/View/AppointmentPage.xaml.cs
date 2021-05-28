using Model;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PeriodPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        private Referral referral;

        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        private Period period;

        public AppointmentPage(Period period)
        {
            InitializeComponent();

            this.DataContext = new AppointmentViewModel(period);

            //if (DateTime.Now >= period.StartTime)
            //{
            //    DoctorsComboBox.IsEnabled = false;
            //    PatientsComboBox.IsEnabled = false;
            //    AppointmentDatePicker.IsEnabled = false;
            //    StartTimeTextBox.IsEnabled = false;
            //    DurationTextBox.IsEnabled = false;
            //    RoomsComboBox.IsEnabled = false;
            //    CancelAppointmentButton.IsEnabled = false;
            //}
            //else
            //{
            //    AnamnesisButton.IsEnabled = false;
            //    PrescriptionButton.IsEnabled = false;
            //}

            //if (period.ReferringReferralId != -1)
            //{
            //    if (Model.Resources.referrals == null)
            //        Model.Resources.OpenReferrals();

            //    foreach (Referral r in Model.Resources.referrals)
            //        if (r.ReferralId == period.ReferringReferralId)
            //        { 
            //            referral = r;
            //            break;
            //        }

            //    SeeReferralButton.Visibility = Visibility.Visible;
            //    DoctorsComboBox.IsHitTestVisible = false;
            //    DoctorsComboBox.IsTabStop = false;
            //    PatientsComboBox.IsHitTestVisible = false;
            //    PatientsComboBox.IsTabStop = false;
            //}
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            //Period editedPeriod = new Period()
            //{
            //    StartTime = dateTime,
            //    Duration = Int32.Parse(DurationTextBox.Text),
            //    PeriodType = PeriodType.APPOINTMENT,
            //    PatientUsername = (PatientsComboBox.SelectedItem as Patient).Username,
            //    DoctorUsername = (DoctorsComboBox.SelectedItem as Doctor).Username,
            //    RoomId = (RoomsComboBox.SelectedItem as Room).Id,
            //    ReferringReferralId = period.ReferringReferralId,
            //    ReferredReferralId = period.ReferredReferralId,
            //    IsUrgent = (bool)IsUrgentCheckBox.IsChecked
            //};
        }

        private void CancelAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to cancel the appointment?\nThis action cannot be undone.",
                                                      "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);
            if (result == MessageBoxResult.Yes)
            {
                if (period.ReferringReferralId != -1)
                {
                    referral.Period = null;
                    referral.IsUsed = false;
                    Model.Resources.SaveReferrals();
                }

                foreach (Period existingPeriod in Model.Resources.periods)
                {
                    if (existingPeriod.RoomId == this.period.RoomId && existingPeriod.StartTime == this.period.StartTime)
                    {
                        Model.Resources.periods.Remove(existingPeriod);
                        break;
                    }
                }

                Model.Resources.SavePeriods();

                MessageBox.Show("Appointment canceled successfully.", "Success");
                NavigationService.GoBack();
            }
        }

        private void AnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PeriodDetailsPage(this.period));
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PatientInfoPage(PatientsComboBox.SelectedItem as Patient));
        }

        private void PrescriptionButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PrescriptionPage(this.period));
        }

        private void ReferralButton_Click(object sender, RoutedEventArgs e)
        {
            Doctor referringDoctor = DoctorsComboBox.SelectedItem as Doctor;
            Patient patient = PatientsComboBox.SelectedItem as Patient;
            NavigationService.Navigate(new ReferralPage(referringDoctor, patient, period));
        }

        private void SeeReferralButton_Click(object sender, RoutedEventArgs e)
        {
            Model.Resources.OpenReferrals();
            Referral referral = Model.Resources.referrals.Find(r => r.ReferralId == period.ReferringReferralId);
            Patient patient = PatientsComboBox.SelectedItem as Patient;
            NavigationService.Navigate(new ReferralPage(referral, patient));
        }
    }
}
