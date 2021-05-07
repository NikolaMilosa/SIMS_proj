using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for UrgentPeriodSummaryPage.xaml
    /// </summary>
    public partial class UrgentPeriodSummaryPage : Page
    {
        private Model.Period _selectedPeriod;
        public Model.Period SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged("SelectedPeriod");
            }
        }
        private Model.Doctor _doctor;
        public Model.Doctor Doctor
        {
            get { return _doctor; }
            set
            {
                _doctor = value;
                OnPropertyChanged("Doctor");
            }
        }

        private Model.Patient _patient;
        public Model.Patient Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged("Patient");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public UrgentPeriodSummaryPage(Model.Period selectedPeriod)
        {
            SelectedPeriod = selectedPeriod;
            Doctor = findDoctorByUsername(SelectedPeriod.DoctorUsername);
            Patient = findPatientByUsername(SelectedPeriod.PatientUsername);

            this.DataContext = this;
            InitializeComponent();
        }

        private Doctor findDoctorByUsername(string username)
        {
            Model.Resources.DeserializeDoctors();
            foreach (KeyValuePair<string, Doctor> item in Model.Resources.doctors)
            {
                if (item.Key.Equals(username))
                {
                    return item.Value;
                }
            }
            return null;
        }

        private Patient findPatientByUsername(string username)
        {
            Model.Resources.OpenPatients();
            foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
            {
                if (item.Key.Equals(username))
                {
                    return item.Value;
                }
            }
            return null;
        }

        private void SeeAllButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecretaryPeriodsPage());
        }
    }
}
