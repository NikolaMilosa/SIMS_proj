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
    public class GuestAccountVM
    {
        public GuestDTO Guest { get; set; }
        public GuestService GuestService { get; set; }
        public GuestAccountVM(bool urgentlyCreated)
        {
            Guest = new GuestDTO(urgentlyCreated);
            GuestService = new GuestService();
            CreateGuestCommand = new RelayCommand(createGuestExecute);
        }

        public ICommand CreateGuestCommand { get; set; }
        private void createGuestExecute(object parameter)
        {
            bool success = GuestService.ProcessGuestCreation(Guest);
            if (success)
            {
                if (Guest.UrgentlyCreated)
                    SecretaryWindowVM.NavigationService.Navigate(new SecretaryUrgentPeriodPage());
                else
                    SecretaryWindowVM.NavigationService.Navigate(new PatientsView());
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Sorry", "Health card number already exists.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }
    }
}
