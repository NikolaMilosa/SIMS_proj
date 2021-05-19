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
    /// Interaction logic for AppointmentPage.xaml
    /// </summary>
    /// 
   
    public partial class AppointmentPage : Page
    {
        public ObservableCollection<AppointmentView> AppointmentList { get; set; }

        public AppointmentPage(string username)
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
                if (period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration)>DateTime.Now) 
                {
                    AppointmentList.Add(new AppointmentView(period));
                }
            }
        }

        private void RemoveAppointmentDialog(AppointmentView appointmentView)
        {
            RemoveAppointmentDialog removeAppointmentDialog = new RemoveAppointmentDialog();
            removeAppointmentDialog.ShowDialog();
            if (PatientUI.RemoveAppointmentDialog.YesPressed)
                RemoveAppointment(appointmentView);
        }

        private void RemoveAppointment(AppointmentView appointmentView)
        {
            ++PatientWindow.RecentActionsNum;
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.DeleteById(appointmentView.Period.PeriodId);
            AppointmentList.Remove(appointmentView);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validations.Validate.TrollDetected())
                return;

            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            if (appointmentView.Period.StartTime < DateTime.Now.AddDays(2))
                Validations.Validate.ShowOkDialog("Warning", "You can't cancel period within 2 days from it's start!");
            else
                RemoveAppointmentDialog(appointmentView);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            if (Validations.Validate.TrollDetected())
                return;

            if (appointmentView.Period.StartTime < DateTime.Now.AddDays(2))
               Validations.Validate.ShowOkDialog("Warning", "You can't edit period within 2 days from it's start!");
            else
                NavigationService.Navigate(new AddAppointmentPage(appointmentView.Period, false, null));
        }
    }
}
