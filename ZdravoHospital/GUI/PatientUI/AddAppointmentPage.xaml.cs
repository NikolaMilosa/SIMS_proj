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
    public partial class AddAppointmentPage : Page
    {

        public ObservableCollection<DoctorView> DoctorList { get; set; }
        public ObservableCollection<TimeSpan> PeriodList { get; set; }
        public Period Period { get; set; }
        AddAppointmentValidations Validations { get; set; }
        public  bool Mode { get; set; }//true=add,false=edit

        public AddAppointmentPage(Period period,bool mode,string username)
        {  
            InitializeComponent();
            Validations = new AddAppointmentValidations(this);
            DataContext = this;
            Mode = mode;
            PeriodList = new ObservableCollection<TimeSpan>();

            Validate.GenerateObesrvableTimes(PeriodList);
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

       

        private void SuggestButton_Click(object sender, RoutedEventArgs e)
        {
  
            SuggestAppointmentValidations suggestValidations = new SuggestAppointmentValidations(this);

            if (suggestValidations.IsOnlyDoctorSelected()) //suggest time based on doctor
                suggestValidations.SuggestTime();
            else if (suggestValidations.IsOnlyTimeSelected())//suggest doctor based on time
            {
                Validations.FillDoctorList();
                suggestValidations.SuggestDoctors();
            }
            else
                Validate.ShowOkDialog("Warning", "Please choose doctor or time so the system could suggest you  periods!");
            
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage(Period.PatientUsername));
        }
    }
}
