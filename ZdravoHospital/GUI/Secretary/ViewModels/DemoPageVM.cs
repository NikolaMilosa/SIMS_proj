using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class DemoPageVM
    {
        public DemoPageVM()
        {
            RegistrationDemoCommand = new RelayCommand(registrationDemoExecute);
            PeriodDemoCommand = new RelayCommand(periodDemoExecute);
        }
        public ICommand RegistrationDemoCommand { get; set; }
        public ICommand PeriodDemoCommand { get; set; }
        private void registrationDemoExecute(object parameter)
        {
            SecretaryWindowVM.NavigationService.Navigate(new PatientRegistrationPage(true));
        }

        private void periodDemoExecute(object parameter)
        {
            SecretaryWindowVM.NavigationService.Navigate(new SecretaryNewPeriodPage(true));
        }
    }
}
