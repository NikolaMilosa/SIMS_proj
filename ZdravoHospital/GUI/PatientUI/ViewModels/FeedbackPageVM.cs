using Model;
using Repository.FeedbackPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
     public class FeedbackPageVM
    {
        #region Properties
        public ObservableCollection<FeedbackType> FeedbackTypes { get; private set; }
        public FeedbackType? SelectedType { get;  set; }
        public Feedback Feedback { get;  set; }
        #endregion

        #region Constructor
        public FeedbackPageVM()
        {
            SetProperties();
            SetCommands();
        }
        #endregion

        #region Commands

        public RelayCommand SubmitCommand { get; private set; }

        #endregion

        #region CommandActions

        private void SubmitExecute(object parameter)
        {
            FeedbackService feedbackFunctions = new FeedbackService(new FeedbackRepository());
            Feedback.Type = (FeedbackType)SelectedType;
            feedbackFunctions.Save(Feedback);
            SuccesfullySubmited();
        }

        private bool SubmitCanExecute(object paramater)
        {
            return SelectedType != null && !String.IsNullOrEmpty(Feedback.Text);
        }
        #endregion

        #region Methods

        private void SetProperties()
        {
            FillTypeCollection();
            Feedback = new Feedback();
            Feedback.SenderUsername = PatientWindowVM.PatientUsername;
            Feedback.Id = Guid.NewGuid();
        }

        private void SetCommands()
        {
            SubmitCommand = new RelayCommand(SubmitExecute, SubmitCanExecute);
        }

        private void FillTypeCollection()
        {
            FeedbackTypes = new ObservableCollection<FeedbackType>();
            FeedbackTypes.Add(FeedbackType.FAULT);
            FeedbackTypes.Add(FeedbackType.IMPROVEMENT);
            FeedbackTypes.Add(FeedbackType.QUESTION);
        }

        private void SuccesfullySubmited()
        {
            ViewService viewFunctions = new ViewService();
            viewFunctions.ShowOkDialog("Feedback","Feedback successfully submited!");
            PatientWindowVM.NavigationService.Navigate(new PeriodPage(PatientWindowVM.PatientUsername));
        }

        #endregion
    }
}
