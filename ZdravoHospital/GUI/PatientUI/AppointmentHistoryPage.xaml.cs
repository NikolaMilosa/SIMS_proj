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
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for AppointmentHistoryPage.xaml
    /// </summary>
    public partial class AppointmentHistoryPage : Page
    {
        public ObservableCollection<AppointmentView> AppointmentList { get; set; }
        public AppointmentHistoryPage(string username)
        {
            InitializeComponent();
            FillList(username);
            DataContext = this;
        }

        private void FillList(string username)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            //Model.Resources.OpenPeriods();
            AppointmentList = new ObservableCollection<AppointmentView>();
            //foreach (Period period in Model.Resources.periods)
            foreach (Period period in periodRepository.GetValues())
            {
                if (period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration) < DateTime.Now)
                {
                    AppointmentList.Add(new AppointmentView(period));
                }
            }
        }

        private void AnamnesisButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            if(appointmentView.Period.Details==null)
            {
                appointmentView.Period.Details = "No available anamnesis for selected appointment!";
            }
            NavigationService.Navigate(new AnamnesisPage(appointmentView.Period.Details, appointmentView.Period.PatientUsername));
        }

        private void RateButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            NavigationService.Navigate(new EvaluateAppointmentPage(appointmentView));
        }
    }
}
