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
        public Thickness TopPanelMargin { get; set; }

        public NewAppointmentPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();
            DataContext = new NewAppointmentViewModel(doctor, startTime, duration);
        }

        public NewAppointmentPage(Referral referral, Patient patient)
        {
            InitializeComponent();
            DataContext = new NewAppointmentViewModel(referral, patient);
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
