using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.PatientUI.Commands;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class TherapyDialogVM
    {
        public string Note { get; set; }
        public string Instructions { get; set; }

        public TherapyDialogVM(string note, string instructions)
        {
            Note = note;
            Instructions = instructions;
            OkCommand = new RelayCommand(OkExecute);
        }

        public RelayCommand OkCommand { get; private set; }

        public void OkExecute(object parameter)
        {
            Window window = (Window)parameter;
            window.Close();
        }
    }
}
