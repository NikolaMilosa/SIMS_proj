using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using ZdravoHospital.GUI.DoctorUI.Commands;
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
        private PeriodReportService _periodReportService;

        public Patient Patient { get; set; }
        public List<PatientInfoPeriodDisplayDTO> PeriodDisplays { get; set; }

        private Visibility _messagePopUpVisibility;
        public Visibility MessagePopUpVisibility
        {
            get
            {
                return _messagePopUpVisibility;
            }
            set
            {
                _messagePopUpVisibility = value;
                OnPropertyChanged("MessagePopUpVisibility");
            }
        }

        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        public MyICommand CloseMessagePopUpCommand { get; set; }

        public void Executed_CloseMessagePopUpCommand()
        {
            MessagePopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_CloseMessagePopUpCommand()
        {
            return true;
        }

        public PatientInfoPage(Patient patient)
        {
            InitializeComponent();

            this.DataContext = this;

            InitializeCommands();

            _periodService = new PeriodService();
            Patient = patient;
            PeriodDisplays = _periodService.GetPatientInfoPeriodDisplayDTOs(Patient.Username);
            PeriodsListView.ItemsSource = PeriodDisplays;

            PeriodTypeComboBox.Items.Add("All");
            PeriodTypeComboBox.Items.Add("Appointments");
            PeriodTypeComboBox.Items.Add("Operations");
            PeriodTypeComboBox.SelectedIndex = 0;

            MessagePopUpVisibility = Visibility.Collapsed;
        }

        private void InitializeCommands()
        {
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
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
            string filename = _periodReportService.GenerateReportFilename(period);
            var p = new System.Diagnostics.Process();

            try
            {
                p.StartInfo = new System.Diagnostics.ProcessStartInfo(Path.GetFullPath(filename))
                {
                    UseShellExecute = true
                };
                p.Start();
            }
            catch (Exception)
            {
                MessageText = "No report generated.";
                MessagePopUpVisibility = Visibility.Visible;
            }
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
