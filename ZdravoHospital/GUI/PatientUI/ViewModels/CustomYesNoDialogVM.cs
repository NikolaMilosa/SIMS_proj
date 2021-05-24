using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.PatientUI.Commands;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class CustomYesNoDialogVM
    {
        public static bool YesPressed { get; set; }
        public string DTitle { get; private set; }
        public string DContent { get; private set; }
        public Window Dialog { get; private set; }

        public CustomYesNoDialogVM(string title, string content,Window dialog)
        {
            DTitle = title;
            DContent = content;
            Dialog = dialog;
            YesCommand = new RelayCommand(YesExecuted);
            NoCommand = new RelayCommand(NoExecuted);
        }

        public RelayCommand YesCommand { get; private set; }
        public RelayCommand NoCommand { get; private set; }

        private void YesExecuted(object parameter)
        {
            YesPressed = true;
            Dialog.Close();
        }

        private void NoExecuted(object parameter)
        {
            YesPressed = true;
            Dialog.Close();
        }
    }
}
