using Model;
using Repository.FeedbackPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class FeedbackLogics
    {
        private IFeedbackRepository _feedbackRepo;

        public FeedbackLogics(IFeedbackRepository repository)
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
