using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class AnamnesisPageVM
    {
        public string AnamnesisContent { get; set; }

        public AnamnesisPageVM(string anamnesisContent)
        {
            AnamnesisContent = anamnesisContent;
            BackCommand = new RelayCommand(BackExecute);
        }

        public RelayCommand BackCommand { get; private set; }

        private void BackExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new AppointmentHistoryPage(PatientWindowVM.PatientUsername));
        }
    }
}
