using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.PatientUI.Commands;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class CustomOkDialogVM
    {
        public string DialogTitle { get; set; }
        public string DialogContent { get; set; }

        public CustomOkDialogVM(string dialogTitle, string dialogContent)
        {
            DialogTitle = dialogTitle;
            DialogContent = dialogContent;
            OkCommand = new RelayCommand(OkExecute);
        }

        public RelayCommand OkCommand { get; private set; }

        public void OkExecute(object parameter)
        {
            Window window = (Window) parameter;
            window.Close();
        }
    }
}
