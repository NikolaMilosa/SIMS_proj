using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.View;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class ViewFunctions
    {
        public bool YesPressed { get; private set; }
        public  void ShowOkDialog(string title, string content)
        {
            CustomOkDialog customOkDialog = new CustomOkDialog(title, content);
            customOkDialog.ShowDialog();
        }

        public void ShowYesNoDialog(string title, string content)
        {
            CustomYesNoDialog customYesNo = new CustomYesNoDialog(title, content);
            customYesNo.ShowDialog();
            YesPressed = CustomYesNoDialogVM.YesPressed;
        }

        public void ShowTherapyDialog(string note, string instructions)
        {
            TherapyDialog customOkDialog = new TherapyDialog(note, instructions);
            customOkDialog.ShowDialog();
        }

        public void ShowPeriodDialog(int id)
        {
            PeriodDetailsWindow periodDetailsWindow = new PeriodDetailsWindow(id);
            periodDetailsWindow.ShowDialog();
        }

    }
}
