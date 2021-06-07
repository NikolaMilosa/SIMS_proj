using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.DTOs;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PatientInfoPage.xaml
    /// </summary>
    public partial class PatientInfoPage : Page, INotifyPropertyChanged
    {
        private PeriodService _periodService;

        public Patient Patient { get; set; }
        public List<PatientInfoPeriodDisplayDTO> PeriodDisplays { get; set; }

        public PatientInfoPage(Patient patient)
        {
            InitializeComponent();

            this.DataContext = this;

            _periodService = new PeriodService();
            Patient = patient;
            PeriodDisplays = _periodService.GetPatientInfoPeriodDisplayDTOs(Patient.Username);
            PeriodsListView.ItemsSource = PeriodDisplays;

            PeriodTypeComboBox.Items.Add("All");
            PeriodTypeComboBox.Items.Add("Appointments");
            PeriodTypeComboBox.Items.Add("Operations");
            PeriodTypeComboBox.SelectedIndex = 0;
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
            Period period = ((sender as Button).DataContext as PatientInfoPeriodDisplayDTO).Period;
        }

        private void PeriodTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            string selection = PeriodTypeComboBox.SelectedValue.ToString();

            if (selection.Equals("All"))
                PeriodsListView.ItemsSource = PeriodDisplays;
            else if (selection.Equals("Appointments"))
                PeriodsListView.ItemsSource = PeriodDisplays.Where(p => p.Period.PeriodType == PeriodType.APPOINTMENT);
            else if (selection.Equals("Operations"))
                PeriodsListView.ItemsSource = PeriodDisplays.Where(p => p.Period.PeriodType == PeriodType.OPERATION);
        }
    }
}
