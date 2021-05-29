using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class PeriodHistoryPageVM
    {
        #region Properties
        public ObservableCollection<PeriodDTO> Periods { get; set; }
        public PeriodDTO SelectedPeriodDTO { get; set; }

        #endregion
        #region Constructor

        public PeriodHistoryPageVM()
        {
            FillList();
            SetCommands();
        }


        #endregion
        #region Commands

        public RelayCommand AnamnesisCommand { get; private set; }
        public RelayCommand RateCommand { get; private set; }


        #endregion
        #region CommandActions

        public void AnamnesisExecuted(object parameter)
        {
            Period selectedPeriod = GetSelectedPeriod();
            if (selectedPeriod.Details == null)
            {
                ViewFunctions viewFunctions = new ViewFunctions();
                viewFunctions.ShowOkDialog("Anamnesis","There is no available anamnesis for the selected appointment!");
                return;

            }
            PatientWindowVM.NavigationService.Navigate(new AnamnesisPage(selectedPeriod.Details));
        }
        public void RateExecuted(object parameter)
        {
            Period selectedPeriod = GetSelectedPeriod();
            PatientWindowVM.NavigationService.Navigate(new EvaluateAppointmentPage(selectedPeriod));

        }

        #endregion
        #region Methods

        private void SetCommands()
        {
            AnamnesisCommand = new RelayCommand(AnamnesisExecuted);
            RateCommand = new RelayCommand(RateExecuted);
        }
        private void FillList()
        {
            PeriodFunctions periodFunctions = new PeriodFunctions();
            Periods = new ObservableCollection<PeriodDTO>();
            PeriodConverter periodConverter = new PeriodConverter();
            foreach (var period in periodFunctions.GetAllPeriods().Where(period => period.PatientUsername.Equals(PatientWindowVM.PatientUsername) && period.StartTime.AddMinutes(period.Duration) < DateTime.Now))
            {
                Periods.Add(periodConverter.GetPeriodDTO(period));
            }
        }
        private Period GetSelectedPeriod()
        {
            PeriodConverter periodConverter = new PeriodConverter();
            return periodConverter.GetPeriod(SelectedPeriodDTO);
        }


        #endregion

    }
}
