using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PeriodPageVM : ViewModel
    {
        #region Properties

        public ObservableCollection<PeriodDTO> PeriodDTOs { get; private set; }
        public PeriodDTO SelectedPeriodDTO { get; set; }
        public PeriodService PeriodFunctions { get; private set; }
        public PeriodConverter PeriodConventer { get; private set; }
        

        #endregion

        #region Fields

        private ViewService viewFunctions;

        private PatientService patientFunctions;
        #endregion

        #region Constructors

        public PeriodPageVM(string username)
        {
            SetFields(username);
            SetProperties(username);
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand EditPeriodCommand { get; private set; }
        public RelayCommand RemovePeriodCommand { get; private set; }

        public RelayCommand SwitchViewCommand { get; private set; }

        private void SwitchViewExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new PeriodCalendarPage());
        }

        public void EditExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;
            Period selectedPeriod = PeriodConventer.GetPeriod(SelectedPeriodDTO);
            PatientWindowVM.NavigationService.Navigate(new AddAppointmentPage(selectedPeriod));
        }

        public bool EditCanExecute(object parameter)
        {
            return !patientFunctions.IsTrollDetected();
        }

        public void RemoveExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;


            viewFunctions.ShowYesNoDialog("Remove appointment", "Are you sure you want to remove appointment?");
            if (viewFunctions.YesPressed)
                RemovePeriod();
            
        }

        #endregion

        #region Methods

        private void SetFields(string username)
        {
            patientFunctions = new PatientService(username);
            viewFunctions = new ViewService();
            
        }

        private void RemovePeriod()
        {
            if (!patientFunctions.ActionTaken())
                return;
            ViewService viewFunctions = new ViewService();
            viewFunctions.ShowOkDialog("Remove appointment", "Appointment succesfully removed!");
            PeriodService periodFunctions = new PeriodService();
            periodFunctions.RemovePeriodById(SelectedPeriodDTO.PeriodId);
            PeriodDTOs.Remove(SelectedPeriodDTO);
        }

        private bool IsPeriodWithin2Days()
        {
            if (SelectedPeriodDTO.Date >= DateTime.Now.AddDays(2)) return false;
            viewFunctions.ShowOkDialog("Warning", "You can't manipulate period 2 days from its start!");
            return true;

        }

        private void SetCommands()
        {
            EditPeriodCommand = new RelayCommand(EditExecute, EditCanExecute);
            RemovePeriodCommand = new RelayCommand(RemoveExecute, EditCanExecute);
            SwitchViewCommand = new RelayCommand(SwitchViewExecute);
        }

        private void SetProperties(string username)
        {
            PeriodFunctions = new PeriodService();
            PeriodConventer = new PeriodConverter();
            PeriodDTOs = new ObservableCollection<PeriodDTO>();
            FillList(username);
        }

        private void FillList(string username)
        {
            foreach (var period in PeriodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration) > DateTime.Now))
                PeriodDTOs.Add(PeriodConventer.GetPeriodDTO(period));
        }

        #endregion

    }
}
