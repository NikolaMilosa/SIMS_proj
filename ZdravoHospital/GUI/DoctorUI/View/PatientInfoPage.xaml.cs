using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Controllers;
using ZdravoHospital.GUI.DoctorUI.DTOs;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PatientInfoPage.xaml
    /// </summary>
    public partial class PatientInfoPage : Page, INotifyPropertyChanged
    {
        private PeriodController _periodController;

        public Patient Patient { get; set; }
        public List<PatientInfoPeriodDisplayDTO> PeriodDisplays { get; set; }
        public PatientInfoPeriodDisplayDTO SelectedPeriod { get; set; }

        public PatientInfoPage(Patient patient)
        {
            InitializeComponent();

            this.DataContext = this;

            _periodController = new PeriodController();
            Patient = patient;
            PeriodDisplays = _periodController.GetPatientInfoPeriodDisplayDTOs(Patient.Username);
            PeriodsListView.ItemsSource = PeriodDisplays;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AlergiesButton_Click(object sender, RoutedEventArgs e)
        {
            AlergiesPopUp.Visibility = Visibility.Visible;
        }

        private void CloseAllergiesPopUpButton_Click(object sender, RoutedEventArgs e)
        {
            AlergiesPopUp.Visibility = Visibility.Hidden;
        }

        private void PeriodDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedPeriod = PeriodsListView.SelectedItem as PatientInfoPeriodDisplayDTO;
            OnPropertyChanged("SelectedPeriod");
            PeriodDetailsPopUp.Visibility = Visibility.Visible;
        }

        private void ClosePeriodDetailsPopUpButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodDetailsPopUp.Visibility = Visibility.Hidden;
        }
    }
}
