using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class ThreadService
    {
        private string username;
        public Thread NotificationThread { get; private set; }
        public Thread NoteThread { get; private set; }
        public Thread TrollThread { get; private set; }

        public ThreadService(string username)
        {
            this.username = username;
        }
        public void StartThreads()
        {
            StartNotificationThread();
            StartTrollThread();
            StartNoteThread();
        }

        public void KillThreads()
        {
            
            //NoteThread.Interrupt();
            //NotificationThread.Interrupt();
            //TrollThread.Interrupt();
        }
        public static void SleepForGivenMinutes(int minutes)
        {
         
                Thread.Sleep(TimeSpan.FromMinutes(minutes));
        }

        private void StartNotificationThread()
        {
            NotificationThread = new Thread(new ParameterizedThreadStart(ThreadTherapyService.TherapyNotification));
            NotificationThread.SetApartmentState(ApartmentState.STA);
            NotificationThread.Start(username);
        }

        private void StartNoteThread()
        {
            NoteThread = new Thread(new ParameterizedThreadStart(ThreadNoteService.NoteNotification));
            NoteThread.SetApartmentState(ApartmentState.STA);
            NoteThread.Start(username);
        }

        private void StartTrollThread()
        {
            TrollThread = new Thread(new ParameterizedThreadStart(ThreadTrollService.ResetActionsNum));
            TrollThread.SetApartmentState(ApartmentState.STA);
            TrollThread.Start(username);
        }
    }
}
