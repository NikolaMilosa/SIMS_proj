using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ZdravoHospital.GUI.DoctorUI
{
    public class PeriodDisplay
    {
        public Period Period { get; }
        public Doctor Doctor { get; }

        public PeriodDisplay(Period period, Doctor doctor)
        {
            Period = period;
            Doctor = doctor;
        }
    }

    /// <summary>
    /// Interaction logic for PatientInfoPage.xaml
    /// </summary>
    public partial class PatientInfoPage : Page, INotifyPropertyChanged
    {
        public Patient Patient { get; set; }
        public List<PeriodDisplay> PeriodDisplays { get; set; }
        public PeriodDisplay SelectedPeriod { get; set; }

        public PatientInfoPage(Patient patient)
        {
            InitializeComponent();

            this.DataContext = this;
            
            Patient = patient;
            PeriodDisplays = new List<PeriodDisplay>();

            foreach (Period period in Model.Resources.periods)
                if (period.PatientUsername.Equals(patient.Username))
                    PeriodDisplays.Add(new PeriodDisplay(period, Model.Resources.doctors[period.DoctorUsername]));

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
            SelectedPeriod = PeriodsListView.SelectedItem as PeriodDisplay;
            OnPropertyChanged("SelectedPeriod");
            PeriodDetailsPopUp.Visibility = Visibility.Visible;
        }

        private void ClosePeriodDetailsPopUpButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodDetailsPopUp.Visibility = Visibility.Hidden;
        }
    }
}
