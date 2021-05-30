using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class NotesPageVM
    {
        #region Properties
        public ObservableCollection<PatientNote> ObservableNotes { get; set; }

        public Patient Patient { get; set; }
        public PatientNote PatientNote { get; set; }

        public PatientFunctions PatientFunctions { get; private set; }

        public ViewFunctions ViewFunctions { get; private set; }


        #endregion

        #region Constructors

        public NotesPageVM()
        {
            SetProperties();
            SetCommands();
        }


        #endregion

        #region Commands

        public RelayCommand RemoveCommand { get; private set; }
        public RelayCommand AddNoteCommand { get; private set; }
        public RelayCommand DoubleClickCommand { get; private set; }

        #endregion

        #region CommandActions

        public void RemoveExecute(object parameter)
        {
            if (!RemoveDialog())
                return;
            RemoveNote();
            ViewFunctions.ShowOkDialog("Removed", "Note succesffuly removed!");
        }

        private void DoubleClickExecute(object sender)
        {
            PatientWindowVM.NavigationService.Navigate(new NoteDetailsPage(PatientNote));
        }

        private void AddNoteExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new CreateNotePage());
        }


        #endregion

        #region Methods

        private bool RemoveDialog()
        {
            ViewFunctions.ShowYesNoDialog("Remove note",
                "Are you sure that you want to remove the selected note?");
            return ViewFunctions.YesPressed;
        }
        private void RemoveNote()
        {
            Patient.PatientNotes.Remove(PatientNote);
            PatientFunctions.SerializePatient(Patient);
            ObservableNotes.Remove(PatientNote);
        }
        private void SetCommands()
        {
            RemoveCommand = new RelayCommand(RemoveExecute);
            AddNoteCommand = new RelayCommand(AddNoteExecute);
            DoubleClickCommand = new RelayCommand(DoubleClickExecute);
        }
        private void SetProperties()
        {
            PatientFunctions = new PatientFunctions(PatientWindowVM.PatientUsername);
            Patient = PatientFunctions.LoadPatient();
            ObservableNotes = new ObservableCollection<PatientNote>(Patient.PatientNotes);
            ViewFunctions = new ViewFunctions();
        }


        #endregion


    }
}
