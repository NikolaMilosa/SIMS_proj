using Model;
using System;

namespace Repository.PersonNotificationPersistance
{
   public interface IPersonNotificationRepository : IRepository<int, PersonNotification>
   {
   }
}