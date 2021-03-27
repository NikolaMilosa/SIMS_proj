using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for SecretaryHomePage.xaml
    /// </summary>
    public partial class SecretaryHomePage : Page
    {
        public SecretaryHomePage()
        {
            InitializeComponent();
        }

        private void btnAddPatient_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new PatientRegistrationPage());
        }

        private void btnSeePatients_Click(object sender, RoutedEventArgs e)
        {
            var window = new PatientsView();
            window.Show();
        }

        private void btnAddGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestAccountPage());
        }
    }
}
