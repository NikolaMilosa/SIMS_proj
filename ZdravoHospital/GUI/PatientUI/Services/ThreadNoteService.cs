using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadNoteService
    {
        public static void NoteNotification(object username)
        {
            while (true)
            {
                PatientService patientFunctions = new PatientService((string)username);
                Patient patient = patientFunctions.LoadPatient();
                GenerateNoteNotificationDialogs(patient.PatientNotes,(string)username);
                ThreadService.SleepForGivenMinutes(1);
            }
        }

        private static void GenerateNoteNotificationDialogs(List<PatientNote> patientNotes,string username)
        {
            PeriodService periodFunctions = new PeriodService();
            foreach (PatientNote note in patientNotes)
            {
                if (!periodFunctions.IsPeriodWithinGivenMinutes(note.NotifyTime, 1)) continue;
                ViewService viewFunctions = new ViewService();
                viewFunctions.ShowOkDialog(note.Title, note.Content);
            }
        }

       
    }
}
