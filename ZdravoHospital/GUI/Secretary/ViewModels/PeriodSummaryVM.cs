﻿using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class PeriodSummaryVM : BindableBase
    {
        public PeriodsToMoveService PeriodsToMoveService { get; set; }
        private Period _selectedPeriod;
        public Period SelectedPeriod
        {
            get { return _selectedPeriod; }
            set
            {
                _selectedPeriod = value;
                OnPropertyChanged("SelectedPeriod");
            }
        }
        private Doctor _doctor;
        public Doctor Doctor
        {
            get { return _doctor; }
            set
            {
                _doctor = value;
                OnPropertyChanged("Doctor");
            }
        }

        private Patient _patient;
        public Patient Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged("Patient");
            }
        }
        public PeriodSummaryVM(Period selectedPeriod)
        {
            SelectedPeriod = selectedPeriod;
            PeriodsToMoveService = new PeriodsToMoveService();
            Doctor = PeriodsToMoveService.GetDoctorById(SelectedPeriod.DoctorUsername);
            Patient = PeriodsToMoveService.GetPatientById(SelectedPeriod.PatientUsername);

            SeeAllCommand = new RelayCommand(seeAllExecute);
        }

        public ICommand SeeAllCommand { get; set; }

        private void seeAllExecute(object parameter)
        {
            SecretaryWindowVM.NavigationService.Navigate(new SecretaryPeriodsPage());
        }
    }
}
