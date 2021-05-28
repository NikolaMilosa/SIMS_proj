using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class CreateNotePageVM : ViewModel
    {
        #region Properties

        public PatientNote PatientNote { get; set; }
        private string errorMessage;
        public string ErrorMessage
        {
            get => errorMessage;
            set
            {
                errorMessage = value;
                OnPropertyChanged("ErrorMessage");
            }
        }

        #endregion

        #region Constructor

        public CreateNotePageVM()
        {
            SetProperties();
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }

        #endregion

        #region CommandActions

        private void ConfirmExecute(object parameter)
        {
            AddNoteToPatient();
            PatientWindowVM.NavigationService.Navigate(new NotesPage(PatientWindowVM.PatientUsername));
        }

        private bool ConfirmCanExecute(object parameter)
        {
            if (PatientNote.NotifyTime < DateTime.Now)
            {
                ErrorMessage = "Pick an upcoming date!";
                return false;
            }

            ErrorMessage = "";
            return !String.IsNullOrEmpty(PatientNote.Title) && !String.IsNullOrEmpty(PatientNote.Content);

        }

        public void CancelExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new NotesPage(PatientWindowVM.PatientUsername));
        }
        #endregion

        #region Methods

        private void SetCommands()
        {
            ConfirmCommand = new RelayCommand(ConfirmExecute, ConfirmCanExecute);
            CancelCommand = new RelayCommand(CancelExecute);
        }

        private void SetProperties()
        {
            PatientNote = new PatientNote();
            PatientNote.NotifyTime = DateTime.Now;
        }

        private void AddNoteToPatient()
        {
            PatientRepository patientRepository = new PatientRepository();
            Patient patient = patientRepository.GetById(PatientWindowVM.PatientUsername);
            patient.PatientNotes.Add(PatientNote);
            patientRepository.Update(patient);
        }

        #endregion
    }
}
