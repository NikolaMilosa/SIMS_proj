using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class ThreadFunctions
    {
        private string username;

        public ThreadFunctions(string username)
        {
            this.username = username;
        }
        public void StartThreads()
        {
            StartNotificationThread();
            StartTrollThread();
            StartNoteThread();
        }

        public static void SleepForGivenMinutes(int minutes)
        {
            Thread.Sleep(TimeSpan.FromMinutes(minutes));
        }

        private void StartNotificationThread()
        {
            Thread notificationThread = new Thread(new ParameterizedThreadStart(ThreadTherapyFunctions.TherapyNotification));
            notificationThread.SetApartmentState(ApartmentState.STA);
            notificationThread.Start(username);
        }

        private void StartNoteThread()
        {
            Thread notificationNoteThread = new Thread(new ParameterizedThreadStart(ThreadNoteFunctions.NoteNotification));
            notificationNoteThread.SetApartmentState(ApartmentState.STA);
            notificationNoteThread.Start(username);
        }

        private void StartTrollThread()
        {
            Thread trollThread = new Thread(new ParameterizedThreadStart(ThreadTrollFunctions.ResetActionsNum));
            trollThread.SetApartmentState(ApartmentState.STA);
            trollThread.Start(username);
        }
    }
}
