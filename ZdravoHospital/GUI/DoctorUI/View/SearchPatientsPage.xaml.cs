using Model;
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
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for SearchPatientsPage.xaml
    /// </summary>
    public partial class SearchPatientsPage : Page
    {
        private PatientService _patientService;
        private List<Patient> _patients;

        public SearchPatientsPage()
        {
            InitializeComponent();

            _patientService = new PatientService();
            _patients = _patientService.GetPatients();
            PatientsListView.ItemsSource = _patients;
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = (sender as Button).DataContext as Patient;
            NavigationService.Navigate(new PatientInfoPage(patient));
        }
    }
}
