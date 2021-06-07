using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Model;
using Syncfusion.UI.Xaml.Schedule;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PeriodCalendarPageVM
    {
        #region Properties

        public ScheduleAppointmentCollection Periods { get; set; }

        public PeriodFunctions PeriodFunctions { get; private set; }
        #endregion

        #region Constructors

        public PeriodCalendarPageVM()
        {
           SetProperties();
           SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand PeriodCommand { get; private set; }

        public RelayCommand SwitchViewCommand { get; private set; }

        #endregion

        #region CommandActions

        private void PeriodExecute(object parameter)
        {
            SfSchedule scheduler = (SfSchedule)parameter;
            int periodID = Int32.Parse(scheduler.SelectedAppointment.Notes);
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowPeriodDialog(periodID);
        }

        private void SwitchViewExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(PatientWindowVM.PatientUsername));
        }

        #endregion

        #region Methods

        private void SetProperties()
        {
            Periods = new ScheduleAppointmentCollection();
            PeriodFunctions = new PeriodFunctions();
            FillCollection();

        }

        private void SetCommands()
        {
            PeriodCommand = new RelayCommand(PeriodExecute);
            SwitchViewCommand = new RelayCommand(SwitchViewExecute);
        }

        private void FillCollection()
        {
            foreach (var period in PeriodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(PatientWindowVM.PatientUsername) && period.StartTime.AddMinutes(period.Duration) > DateTime.Now))
                GenerateScheduleAppointment(period);
        }

        private void GenerateScheduleAppointment(Period period)
        {
            ScheduleAppointment appointment = new ScheduleAppointment
            {
                Subject = period.PeriodType==PeriodType.APPOINTMENT? "Appointment" : "Operation",
                StartTime = period.StartTime,
                EndTime = period.StartTime.AddMinutes(period.Duration),
                Notes = period.PeriodId.ToString(),
            };
            Periods.Add(appointment);
        }
        

        #endregion
    }
}
