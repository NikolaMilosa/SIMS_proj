using Model;
using Repository.NotificationsPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZdravoHospital.GUI.DoctorUI.DTOs;

namespace ZdravoHospital.GUI.DoctorUI.Services
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

        public List<NotificationDisplayDTO> GetNotificationDTOs(string username)
        {
            List<NotificationDisplayDTO> userNotifications = new List<NotificationDisplayDTO>();
            List<PersonNotification> personNotifications = _personNotificationRepository.GetValues().Where(n => n.Username.Equals(username)).ToList();

            foreach (Notification notification in _notificationRepository.GetValues())
            {
                PersonNotification personNotification = personNotifications.Find(n => n.NotificationId == notification.NotificationId);
                
                if (personNotification != null)
                    userNotifications.Add(new NotificationDisplayDTO(notification, personNotification.IsRead));
            }

            return userNotifications;
        }
    }
}
