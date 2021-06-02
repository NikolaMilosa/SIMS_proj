using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.FeedbackPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;

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
        }
    }
}
