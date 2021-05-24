using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class ViewFunctions
    {
        public  void ShowOkDialog(string title, string content)
        {
            CustomOkDialog customOkDialog = new CustomOkDialog(title, content);
            customOkDialog.ShowDialog();
        }

    }
}
