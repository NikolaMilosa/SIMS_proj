using Model;
using Repository.FeedbackPersistance;
using System;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class FeedbackService
    {
        private FeedbackRepository _feedbackRepository;

        public FeedbackService()
        {
            _feedbackRepository = new FeedbackRepository();
        }

        public void SendFeedback(Feedback feedback)
        {
            feedback.Id = Guid.NewGuid();
            _feedbackRepository.Create(feedback);
        }
    }
}
