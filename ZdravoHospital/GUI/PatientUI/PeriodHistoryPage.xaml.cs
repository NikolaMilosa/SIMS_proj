using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for AppointmentHistoryPage.xaml
    /// </summary>
    public partial class AppointmentHistoryPage : Page
    {
        public ObservableCollection<PeriodDTO> Periods { get; set; }
        public Model.Period SelectedPeriod { get; set; }
        public AppointmentHistoryPage(string username)
        {
            InitializeComponent();
            FillList(username);
            DataContext = this;
        }

        private void FillList(string username)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            Periods = new ObservableCollection<PeriodDTO>();
            PeriodConverter periodConverter = new PeriodConverter();
            foreach (Model.Period period in periodRepository.GetValues())
            {
                if (period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration) < DateTime.Now)
                {
                    Periods.Add(periodConverter.GetPeriodDTO(period));
                }
            }
        }

        private void SetSelectedPeriod()
        {
            PeriodDTO period = (PeriodDTO)appointmentDataGrid.SelectedItem;
            PeriodConverter periodConverter = new PeriodConverter();
            SelectedPeriod = periodConverter.GetPeriod(period);
        }

        private void AnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedPeriod();
            if(SelectedPeriod.Details==null)
            {
                SelectedPeriod.Details = "No available anamnesis for selected appointment!";
            }
            NavigationService.Navigate(new AnamnesisPage(SelectedPeriod.Details, SelectedPeriod.PatientUsername));
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            SetSelectedPeriod();
            PeriodDTO period = (PeriodDTO)appointmentDataGrid.SelectedItem;
            NavigationService.Navigate(new EvaluateAppointmentPage(SelectedPeriod));
        }
    }
}
