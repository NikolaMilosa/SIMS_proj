using Model;
using Repository.FeedbackPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class FeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        public FeedbackService()
        {
            _feedbackRepository = new FeedbackRepository();

        }

        public void AddFeedback(Feedback newFeedback)
        {
            _feedbackRepository.Create(newFeedback);
        }
    }
}
