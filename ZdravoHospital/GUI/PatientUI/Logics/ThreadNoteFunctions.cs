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
                GenerateNoteNotificationDialogs(patient.PatientNotes);
                Validate.SleepForGivenMinutes(1);
            }
        }

        private static void GenerateNoteNotificationDialogs(List<PatientNote> patientNotes)
        {
            foreach (PatientNote note in patientNotes)
            {
                if (Validate.IsPeriodWithinGivenMinutes(note.NotifyTime, 1))
                    Validate.ShowOkDialog(note.Title, note.Content);

            }
        }
    }
}
