using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadTrollService
    {
        public static void ResetActionsNum(object patientUsername)
        {
            PatientService patientFunctions = new PatientService((string)patientUsername);
            Patient patient = patientFunctions.LoadPatient();
            SetNumberOfRecentActions(patient);
            while (true)
            {
                ThreadService.SleepForGivenMinutes(5);
                ResetActions(patientFunctions);
            }
        }

        private static void ResetActions(PatientService patientFunctions)
        { 
            Patient patient = patientFunctions.LoadPatient();
            if (patient.RecentActions >= 5) return;
            patient.RecentActions = 0;
            patientFunctions.SerializePatient(patient);
        }

        private static void SetNumberOfRecentActions(Patient patient)
        {
            PatientService patientFunctions = new PatientService(patient.Username);
            if (patient.LastLogoutTime.AddMinutes(5) > DateTime.Now || IsUserBlocked(patient)) return;
            patient.RecentActions = 0;
            patientFunctions.SerializePatient(patient);
        }

        private static bool IsUserBlocked(Patient user)
        {
            return !(user.RecentActions < 5); 
        }

    }
}
