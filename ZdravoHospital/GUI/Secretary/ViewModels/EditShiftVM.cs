using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class EditShiftVM
    {
        public Doctor SelectedDoctor { get; set; }
        public ShiftDTO ShiftDTO { get; set; }
        public ShiftService ShiftService { get; set; }
        public EditShiftVM(Doctor selectedDoctor)
        {
            SelectedDoctor = selectedDoctor;
            ShiftDTO = new ShiftDTO();

            IDoctorRepository doctorRepository = RepositoryFactory.CreateDoctorRepository();
            IPeriodRepository periodRepository = RepositoryFactory.CreatePeriodRepository();
            ShiftService = new ShiftService(doctorRepository, periodRepository);

            EditShiftCommand = new RelayCommand(editShiftExecute);
        }

        public ICommand EditShiftCommand { get; set; }
        private void editShiftExecute(object parameter)
        {
            ShiftService.ProcessShiftCreation(SelectedDoctor, ShiftDTO);
            SecretaryWindowVM.NavigationService.Navigate(new DoctorsView());
        }
    }
}
