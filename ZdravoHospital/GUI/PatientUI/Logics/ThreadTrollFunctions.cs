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
            Patient user = LoadPatient((string)patientUsername);
            SetNumberOfRecentActions(user);
            while (true)
            {
                ThreadFunctions.SleepForGivenMinutes(5);
                if (user.RecentActions >= 5) continue;
                user.RecentActions = 0;
                SerializePatient(user);
            }
        }

        private static void SetNumberOfRecentActions(Patient user)
        {
            if (user.LastLogoutTime.AddMinutes(5) > DateTime.Now || IsUserBlocked(user)) return;
            user.RecentActions = 0;
            SerializePatient(user);
        }

        private static bool IsUserBlocked(Patient user)
        {
            return !(user.RecentActions < 5); 
        }

        private static void SerializePatient(Patient user)
        {
            PatientRepository patientRepository = new PatientRepository();
            patientRepository.Update(user);
        }

        private static Patient LoadPatient(string username)
        {
            PatientRepository patientRepository = new PatientRepository();
            return patientRepository.GetById(username);
        }

    }
}
