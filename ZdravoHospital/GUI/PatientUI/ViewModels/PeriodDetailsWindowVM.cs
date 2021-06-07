using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using Model;
using ZdravoHospital.GUI.DoctorUI.Services;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PeriodDetailsWindowVM
    {
        #region Properties

        public PeriodDTO PeriodDTO { get; set; }
        public PeriodFunctions PeriodService { get; private set; }
        public PeriodConverter PeriodConventer { get; private set; }
        public PatientFunctions PatientFunctions { get; private set; }

        #endregion

        #region Constructors

        public PeriodDetailsWindowVM(int periodID)
        {
            SetProperties(periodID);
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand CancelCommand { get; set; }
        public RelayCommand EditCommand { get; set; }
        public RelayCommand RemoveCommand { get; set; }

        #endregion

        #region CommandActions

        public void CancelExecute(object parameter)
        {
            Window window = (Window) parameter;
            window.Close();
        }

        public void EditExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;
            Period selectedPeriod = PeriodConventer.GetPeriod(PeriodDTO);
            CancelExecute(parameter);
            PatientWindowVM.NavigationService.Navigate(new AddAppointmentPage(selectedPeriod));
        }

        public bool EditCanExecute(object parameter)
        {
            return !PatientFunctions.IsTrollDetected();
        }

        public void RemoveExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;

            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowYesNoDialog("Remove appointment", "Are you sure you want to remove appointment?");
            if (viewFunctions.YesPressed)
                RemovePeriod();
            CancelExecute(parameter);
            PatientWindowVM.NavigationService.Navigate(new PeriodCalendarPage());
        }

        #endregion

        #region Methods

        private void SetCommands()
        {
            CancelCommand = new RelayCommand(CancelExecute);
            EditCommand = new RelayCommand(EditExecute, EditCanExecute);
            RemoveCommand = new RelayCommand(RemoveExecute, EditCanExecute);
        }


        private void SetProperties(int periodID)
        {
            PeriodService = new PeriodFunctions();
            PeriodConventer = new PeriodConverter();
            PatientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            SetPeriodDTO(periodID);
        }

        private void RemovePeriod()
        {
            if (!PatientFunctions.ActionTaken())
                return;
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowOkDialog("Remove appointment", "Appointment succesfully removed!");
            PeriodFunctions periodFunctions = new PeriodFunctions();
            periodFunctions.RemovePeriodById(PeriodDTO.PeriodId);
            
        }

        private bool IsPeriodWithin2Days()
        {
            if (PeriodDTO.Date >= DateTime.Now.AddDays(2)) return false;
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowOkDialog("Warning", "You can't manipulate period 2 days from its start!");
            return true;

        }

        private void SetPeriodDTO(int periodID)
        {
            Period period = PeriodService.GetPeriod(periodID);
            PeriodDTO = PeriodConventer.GetPeriodDTO(period);
        }
        #endregion

    }
}
