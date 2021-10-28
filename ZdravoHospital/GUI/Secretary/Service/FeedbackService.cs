using Model;
using Repository.FeedbackPersistance;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class FeedbackService
    {
        private IFeedbackRepository _feedbackRepository;
        public FeedbackService(IFeedbackRepository feedbackRepository)
        {
            _feedbackRepository = feedbackRepository;

        }

        public void AddFeedback(Feedback newFeedback)
        {
            _feedbackRepository.Create(newFeedback);
        }
    }
}
