using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class NoteDetailsPageVM
    {
        #region Properties
        public PatientNote PatientNote { get; set; }
        #endregion

        #region Constructor

        public NoteDetailsPageVM(PatientNote patientNote)
        {
            PatientNote = patientNote;
            BackCommand = new RelayCommand(BackExecute);
        }

        #endregion

        #region Commands

        public RelayCommand BackCommand { get; private set; }

        #endregion

        #region CommandActions

        private void BackExecute(object sender)
        {
            PatientWindowVM.NavigationService.Navigate(new NotesPage(PatientWindowVM.PatientUsername));
        }

        #endregion

        #region Methods



        #endregion

    }
}
