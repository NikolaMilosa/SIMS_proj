using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.FeedbackPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class FeedbackService
    {
        #region Fields

        private IFeedbackRepository _feedbackRepository;

        #endregion

        public FeedbackService(InjectorDTO injector)
        {
            _feedbackRepository = injector.FeedbackRepository;
        }

        public void SendFeedback(Feedback newFeedback)
        {
            newFeedback.Id = Guid.NewGuid();
            _feedbackRepository.Create(newFeedback);

            var notification = new MyMessageBoxViewModel($"Your feedback has been submitted! Thank you.");
        }
    }
}
