using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTOs;
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
            VacationService = new VacationService();
            VacationDTO = new VacationDTO();

            CreateVacationCommand = new RelayCommand(createVacationExecute);
            ClearVacationsCommand = new RelayCommand(clearVacationsExecute);
        }

        public ICommand CreateVacationCommand { get; set; }
        public ICommand ClearVacationsCommand { get; set; }

        private void createVacationExecute(object parameter)
        {
            VacationService.ProcessVacationCreation(VacationDTO, SelectedDoctor);
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
