using System;
using Model;

namespace Repository.FeedbackPersistance
{
   public interface IFeedbackRepository : IRepository<Guid, Feedback>
   {
   }
}