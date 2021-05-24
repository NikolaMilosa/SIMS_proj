using Model;
using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public static class ThreadNoteFunctions
    {
        public static void NoteNotification(object username)
        {
            while (true)
            {
                Patient patient = Validate.LoadPatient((string)username);
                GenerateNoteNotificationDialogs(patient.PatientNotes,(string)username);
                ThreadFunctions.SleepForGivenMinutes(1);
            }
        }

        private static void GenerateNoteNotificationDialogs(List<PatientNote> patientNotes,string username)
        {
            PeriodFunctions periodFunctions = new PeriodFunctions(username);
            foreach (PatientNote note in patientNotes)
            {
                if (periodFunctions.IsPeriodWithinGivenMinutes(note.NotifyTime, 1))
                    Validate.ShowOkDialog(note.Title, note.Content);

            }
        }
    }
}
