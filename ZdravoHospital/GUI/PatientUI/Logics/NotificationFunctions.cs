using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.NotificationsPersistance;
using Repository.PersonNotificationPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class NotificationFunctions
    {
        public NotificationRepository NotificationRepository { get; private set; }
        public PersonNotificationRepository PersonNotificationRepository { get; private set; }

        public NotificationFunctions()
        {
            NotificationRepository = new NotificationRepository();
            PersonNotificationRepository = new PersonNotificationRepository();
        }

        public List<PersonNotification> GetPersonNotifications()
        {
            return PersonNotificationRepository.GetValues();
        }

        public void UpdatePersonNotification(PersonNotification personNotification)
        {
            PersonNotificationRepository.Update(personNotification);
        }
        public List<Notification> GetNotifications()
        {
            return NotificationRepository.GetValues();
        }
}
}
