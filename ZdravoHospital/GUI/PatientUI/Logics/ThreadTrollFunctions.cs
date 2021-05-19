using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadTrollFunctions
    {
        public static void ResetActionsNum(object patientUsername)
        {
            Patient user = Validate.LoadPatient((string)patientUsername);
            SetNumberOfRecentActions(user);
            while (true)
            {
                Validate.SleepForGivenMinutes(5);
                if (PatientWindow.RecentActionsNum < 5)
                    PatientWindow.RecentActionsNum = 0;
            }
        }

        private static void SetNumberOfRecentActions(Patient user)
        {
            if (user.LastLogoutTime.AddMinutes(5) <= DateTime.Now && !IsUserBlocked(user))
                PatientWindow.RecentActionsNum = 0;
            else
                PatientWindow.RecentActionsNum = user.RecentActions;
        }

        private static bool IsUserBlocked(Patient user)
        {
            bool blocked = true;
            if (user.RecentActions < 5)
                blocked = false;

            return blocked;
        }

    }
}
