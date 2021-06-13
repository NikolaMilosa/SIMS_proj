using Model;
using Repository.FeedbackPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class FeedbackService
    {
        private IFeedbackRepository _feedbackRepo;

        public FeedbackService(IFeedbackRepository repository)
        {
            _feedbackRepo = repository;
        }

        public void Save(Feedback feedback)
        {
            List<Feedback> feedbacks = _feedbackRepo.GetValues();
            feedbacks.Add(feedback);
            _feedbackRepo.Save(feedbacks);
        }

    }
}
