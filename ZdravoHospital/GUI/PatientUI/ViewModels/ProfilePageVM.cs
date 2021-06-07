using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class ProfilePageVM
    {
        public Patient Patient { get; private set; }

        public ProfilePageVM()
        {
            SetProperties();
        }

        private void SetProperties()
        {
            PatientFunctions patientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            Patient = patientFunctions.LoadPatient();
        }
    }
}
