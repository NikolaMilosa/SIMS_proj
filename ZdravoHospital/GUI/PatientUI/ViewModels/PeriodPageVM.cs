using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PeriodPageVM : ViewModel
    {
        #region Properties

        public ObservableCollection<PeriodDTO> PeriodDTOs { get; private set; }
        public PeriodDTO SelectedPeriodDTO { get; set; }
        public PeriodRepository PeriodRepository { get; private set; }
        public PeriodConverter PeriodConventer { get; private set; }
        

        #endregion

        #region Fields

        private ViewFunctions viewFunctions;

        private PatientFunctions patientFunctions;
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

        public void EditExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;
            Period selectedPeriod = PeriodConventer.GetPeriod(SelectedPeriodDTO);
            PatientWindowVM.NavigationService.Navigate(new AddAppointmentPage(selectedPeriod));
        }

        public bool EditCanExecute(object parameter)
        {

            if (patientFunctions.IsTrollDetected())
            {
                return false;
            }


            return true;
        }

        public void RemoveExecute(object parameter)
        {
            if (IsPeriodWithin2Days())
                return;

            //patientFunctions.ActionTaken();
            ViewFunctions viewFunctions = new ViewFunctions();
            viewFunctions.ShowYesNoDialog("Remove appointment", "Are you sure you want to remove appointment?");
            if (viewFunctions.YesPressed)
                RemovePeriod();
        }

        #endregion

        #region Methods

        private void SetFields(string username)
        {
            patientFunctions = new PatientFunctions(username);
            viewFunctions = new ViewFunctions();
            
        }

        private void RemovePeriod()
        {
            patientFunctions.ActionTaken();
            PeriodRepository.DeleteById(SelectedPeriodDTO.PeriodId);
            PeriodDTOs.Remove(SelectedPeriodDTO);
        }

        private bool IsPeriodWithin2Days()
        {
            if (SelectedPeriodDTO.Date < DateTime.Now.AddDays(2))
            {
                viewFunctions.ShowOkDialog("Warning", "You can't manipulate period 2 days from its start!");
                return true;
            }

            return false;
        }

        private void SetCommands()
        {
            EditPeriodCommand = new RelayCommand(EditExecute, EditCanExecute);
            RemovePeriodCommand = new RelayCommand(RemoveExecute, EditCanExecute);
        }

        private void SetProperties(string username)
        {
            PeriodRepository = new PeriodRepository();
            PeriodConventer = new PeriodConverter();
            PeriodDTOs = new ObservableCollection<PeriodDTO>();
            FillList(username);
        }

        private void FillList(string username)
        {
            foreach (var period in PeriodRepository.GetValues().Where(period => period.PatientUsername.Equals(username) && period.StartTime.AddMinutes(period.Duration) > DateTime.Now))
                PeriodDTOs.Add(PeriodConventer.GetPeriodDTO(period));
        }

        #endregion

    }
}
