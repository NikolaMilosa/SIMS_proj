using Model;
using Repository.DoctorPersistance;
using Repository.PeriodPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class EditVacationVM
    {
        public Doctor SelectedDoctor { get; set; }
        public VacationDTO VacationDTO { get; set; }
        public VacationService VacationService { get; set; }
        public EditVacationVM(Doctor selectedDoctor)
        {
            SelectedDoctor = selectedDoctor;

            IPeriodRepository periodRepository = RepositoryFactory.CreatePeriodRepository();
            IDoctorRepository doctorRepository = RepositoryFactory.CreateDoctorRepository();
            VacationService = new VacationService(doctorRepository, periodRepository);
            
            VacationDTO = new VacationDTO();

            CreateVacationCommand = new RelayCommand(createVacationExecute);
            ClearVacationsCommand = new RelayCommand(clearVacationsExecute);
        }

        public ICommand CreateVacationCommand { get; set; }
        public ICommand ClearVacationsCommand { get; set; }

        private void createVacationExecute(object parameter)
        {
            bool success = VacationService.ProcessVacationCreation(VacationDTO, SelectedDoctor);
            if (!success)
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Error", "Too many free days.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
            else 
                SecretaryWindowVM.NavigationService.Navigate(new DoctorsView());
        }

        private void clearVacationsExecute(object parameter)
        {
            VacationService.ProcessVacationDeletion(SelectedDoctor);
            //MessageBox.Show("Deleted successfully!");
            SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Success", "Deleted successfully.");
            SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
            SecretaryWindowVM.CustomMessageBox.Show();
        }
    }
}
