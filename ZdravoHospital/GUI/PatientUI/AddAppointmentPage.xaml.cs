using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
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
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for AddAppointmentPage.xaml
    /// </summary>
    public partial class AddAppointmentPage : Page
    {

        public ObservableCollection<DoctorView> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }
        public Period Period { get; set; }

        public List<TimeSpan> TimeList { get; set; }

        AddAppointmentValidations Validations { get; set; }

        public  bool Mode { get; set; }//true=add,false=edit

        public AddAppointmentPage(Period period,bool mode,string username)
        {  
            InitializeComponent();
            Validations = new AddAppointmentValidations(this);
            DataContext = this;
            Mode = mode;
            PeriodList = new ObservableCollection<TimeSpan>();

            Validate.generateObesrvableTimes(PeriodList);
            Validations.FillDoctorList();
            Validations.GeneratePeriod(period, username);
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Validations.CheckPeriodAvailibility())
                return;
   
            Validations.SerializePeriod();

            ++PatientWindow.RecentActionsNum;
            NavigationService.Navigate(new AppointmentPage(Period.PatientUsername));
        }

       

        private void suggestButton_Click(object sender, RoutedEventArgs e)
        {
            Period period = new Period();
            period.PatientUsername = Period.PatientUsername;
            period.Duration = 30;

            if (selectDoctor.SelectedItem != null && selectDate.SelectedDate == null && selectTime.SelectedItem == null) //suggest time based on doctor
            {
                period.DoctorUsername = ((DoctorView)selectDoctor.SelectedItem).Username;
                Validate.suggestTime(period, PeriodList);
                selectDate.SelectedDate = period.StartTime;
                selectTime.Focus();
                selectTime.IsDropDownOpen = true;
            }
            else if(selectDoctor.SelectedItem == null && selectDate.SelectedDate != null && selectTime.SelectedItem != null)//suggest doctor based on time
            {

                period.StartTime = (DateTime)selectDate.SelectedDate;
                period.StartTime += (TimeSpan)selectTime.SelectedItem;
                if (!Validate.checkPeriod(period, true))
                    return;

                Validations.FillDoctorList();
                Validate.suggestDoctor(period, DoctorList);
                if (DoctorList.Count <= 0)
                {
                    customOkDialog customOkDialog = new customOkDialog("Warning", "There is no available doctor at the selected time!");
                    customOkDialog.ShowDialog();
                }
                else
                {
                    customOkDialog customOkDialog = new customOkDialog("Suggested doctor", "Doctor list is updated to suggested doctors!");
                    customOkDialog.ShowDialog();
                    selectDoctor.IsDropDownOpen = true;
                }
            }
            else
            {
                customOkDialog customOkDialog = new customOkDialog("Warning", "Please choose doctor or time so the system could suggest you  periods!");
                customOkDialog.ShowDialog();
            }
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage(Period.PatientUsername));
        }
    }
}
