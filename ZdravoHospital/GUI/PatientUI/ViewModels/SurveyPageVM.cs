using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class SurveyPageVM : ViewModel
    {
        #region Properties

        private Survey survey;
        public Survey Survey
        {
            get => survey;
            set
            {
                survey = value;
                OnPropertyChanged("Survey");
            }
        }
        public PatientWindowVM PatientWindowVM { get; private set; }
        public ViewFunctions ViewFunctions { get; private set; }
        
        #endregion

        #region Constructor

        public SurveyPageVM(PatientWindowVM patientWindowVm)
        {
            SetProperties(patientWindowVm);
            SetCommands();
        }

        #endregion

        #region Commands

        public RelayCommand SubmitCommand { get; set; }
        public RelayCommand CancelCommand { get; set; }

        #endregion

        #region CommandActions

        public void SubmitExecute(object parameter)
        {
            PatientWindowVM.SurveyAvailable = false;
            SerializeSurvey();
            ViewFunctions.ShowOkDialog("Survey", "Thank you for completing the survey!");
            PatientWindowVM.NavigatonService.Navigate(new PeriodPage(PatientWindowVM.PatientUsername));
        }

        public bool SubmitCanExecute(object parameter)
        {
            if (survey.AppointmentAccessibility == 0 || survey.Care == 0 || survey.Hygiene == 0 ||
                survey.Recommendation == 0)
                return false;
            return true;
        }

        public void CancelExecute(object parameter)
        {
            PatientWindowVM.NavigatonService.Navigate(new PeriodPage(PatientWindowVM.PatientUsername));
        }

        #endregion

        #region Methods

        private void SerializeSurvey()
        {
            Survey.CreationDate = DateTime.Now;
            Survey.PatientUsername = PatientWindowVM.PatientUsername;
            SurveyRepository surveyRepository = new SurveyRepository();
            surveyRepository.Create(Survey);
        }

        private void SetProperties(PatientWindowVM patientWindowVm)
        {
            Survey = new Survey();
            PatientWindowVM = patientWindowVm;
            ViewFunctions = new ViewFunctions();
        }


        private void SetCommands()
        {
            SubmitCommand = new RelayCommand(SubmitExecute, SubmitCanExecute);
            CancelCommand = new RelayCommand(CancelExecute);
        }


        #endregion
    }
}
