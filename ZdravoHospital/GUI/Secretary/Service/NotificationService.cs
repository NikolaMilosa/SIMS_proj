using Model;
using Repository.NotificationsPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class NotificationService
    {
        private NotificationRepository _notificationRepository;
        private PersonNotificationRepository _personNotificationRepository;
        public NotificationService()
        {
            _notificationRepository = new NotificationRepository();
            _personNotificationRepository = new PersonNotificationRepository();
        }
        public List<Notification> GetAllNotifications()
        {
            return _notificationRepository.GetValues();
        }
        public void RemoveNotification(int id)
        {
            _notificationRepository.DeleteById(id);
            _personNotificationRepository.DeleteById(id);
        }
    }
}
