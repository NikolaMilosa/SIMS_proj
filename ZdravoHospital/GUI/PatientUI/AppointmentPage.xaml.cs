using Model;
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
            fillList(username);
            DataContext = this;
        }

        private void fillList(string username)
        {
            Model.Resources.OpenPeriods();
            AppointmentList = new ObservableCollection<AppointmentView>();
            foreach (Period period in Model.Resources.periods)
            {
                if (period.PatientUsername.Equals(username)) 
                {
                    AppointmentList.Add(new AppointmentView(period));
                }
            }
        }

        private void removeButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;
            if (appointmentView.Period.StartTime < DateTime.Now.AddDays(2)) 
            {
                customOkDialog dialog = new customOkDialog("Warning", "You can't cancel period within 2 days from it's start!");
                dialog.Show();
            }
            else 
            {
                RemoveAppointmentDialog removeAppointmentDialog = new RemoveAppointmentDialog();
                removeAppointmentDialog.ShowDialog();
                if(RemoveAppointmentDialog.YesPressed)
                {
                    AppointmentList.Remove(appointmentView);
                    Model.Resources.periods.Remove(appointmentView.Period);
                    Model.Resources.SavePeriods();
                }
            }

        }

        private void editButton_Click(object sender, RoutedEventArgs e)
        {
            AppointmentView appointmentView = (AppointmentView)appointmentDataGrid.SelectedItem;

            if (appointmentView.Period.StartTime < DateTime.Now.AddDays(2))
            {
                customOkDialog dialog = new customOkDialog("Warning", "You can't edit period within 2 days from it's start!");
                dialog.Show();
            }
            else
                NavigationService.Navigate(new AddAppointmentPage(appointmentView.Period, false, null));
        }
    }
}
