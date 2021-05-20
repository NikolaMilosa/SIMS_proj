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
    /// Interaction logic for PeriodPage.xaml
    /// </summary>
    /// 
   
    public partial class PeriodPage : Page
    {
        public ObservableCollection<PeriodDTO> PeriodDTOs { get; set; }
        public Period SelectedPeriod { get; set; }

        public PeriodPage(string username)
        {
            InitializeComponent();
            FillList(username);
            DataContext = this;
        }

        private void FillList(string username)
        {
            PeriodRepository periodRepository = new PeriodRepository();
            PeriodDTOs = new ObservableCollection<PeriodDTO>();
            PeriodConverter periodConverter = new PeriodConverter();

            foreach (Model.Period period in periodRepository.GetValues())
            {
                if (period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration)>DateTime.Now) 
                {
                    PeriodDTOs.Add(periodConverter.GetPeriodDTO(period));
                }
            }
        }

        private void SetSelectedPeriod()
        {
            PeriodDTO period = (PeriodDTO)AppointmentDataGrid.SelectedItem;
            PeriodConverter periodConverter = new PeriodConverter();
            SelectedPeriod = periodConverter.GetPeriod(period);
        }
        private void RemoveAppointmentDialog(PeriodDTO appointmentView)
        {
            RemoveAppointmentDialog removeAppointmentDialog = new RemoveAppointmentDialog();
            removeAppointmentDialog.ShowDialog();
            if (PatientUI.RemoveAppointmentDialog.YesPressed)
                RemoveAppointment(appointmentView);
        }

        private void RemoveAppointment(PeriodDTO appointmentView)
        {
            ++PatientWindow.RecentActionsNum;
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.DeleteById(appointmentView.PeriodId);
            PeriodDTOs.Remove(appointmentView);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            if (Validations.Validate.TrollDetected())
                return;

            PeriodDTO period = (PeriodDTO)AppointmentDataGrid.SelectedItem;
            if (period.Date < DateTime.Now.AddDays(2))
                Validations.Validate.ShowOkDialog("Warning", "You can't cancel period within 2 days from it's start!");
            else
                RemoveAppointmentDialog(period);
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            PeriodDTO period = (PeriodDTO)AppointmentDataGrid.SelectedItem;
            SetSelectedPeriod();
            if (Validations.Validate.TrollDetected())
                return;

            if (period.Date < DateTime.Now.AddDays(2))
               Validations.Validate.ShowOkDialog("Warning", "You can't edit period within 2 days from it's start!");
            else
                NavigationService.Navigate(new AddAppointmentPage(SelectedPeriod, false, null));
        }
    }
}
