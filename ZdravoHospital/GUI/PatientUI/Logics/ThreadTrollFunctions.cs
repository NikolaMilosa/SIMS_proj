using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadTrollFunctions
    {
        public static void ResetActionsNum(object patientUsername)
        {
            PatientFunctions patientFunctions = new PatientFunctions((string)patientUsername);
            Patient patient = patientFunctions.LoadPatient();
            SetNumberOfRecentActions(patient);
            while (true)
            {
                ThreadFunctions.SleepForGivenMinutes(5);
                ResetActions(patientFunctions);
            }
        }

        private static void ResetActions(PatientFunctions patientFunctions)
        { 
            Patient patient = patientFunctions.LoadPatient();
            if (patient.RecentActions >= 5) return;
            patient.RecentActions = 0;
            patientFunctions.SerializePatient(patient);
        }

        private static void SetNumberOfRecentActions(Patient patient)
        {
            PatientFunctions patientFunctions = new PatientFunctions(patient.Username);
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
