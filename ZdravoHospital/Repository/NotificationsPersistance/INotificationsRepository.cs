using Model;
using System;

namespace Repository.NotificationsPersistance
{
   public interface INotificationsRepository : IRepository<int, Notification>
   {
   }
}