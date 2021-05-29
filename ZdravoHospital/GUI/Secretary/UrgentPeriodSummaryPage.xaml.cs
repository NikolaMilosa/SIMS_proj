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
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for UrgentPeriodSummaryPage.xaml
    /// </summary>
    public partial class UrgentPeriodSummaryPage : Page
    {
        public PeriodsToMoveService PeriodsToMoveService { get; set; }
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
        public UrgentPeriodSummaryPage(Period selectedPeriod)
        {
            SelectedPeriod = selectedPeriod;
            PeriodsToMoveService = new PeriodsToMoveService();
            Doctor = PeriodsToMoveService.GetDoctorById(SelectedPeriod.DoctorUsername);
            Patient = PeriodsToMoveService.GetPatientById(SelectedPeriod.PatientUsername);

            this.DataContext = this;
            InitializeComponent();
        }


        private void SeeAllButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecretaryPeriodsPage());
        }
    }
}
