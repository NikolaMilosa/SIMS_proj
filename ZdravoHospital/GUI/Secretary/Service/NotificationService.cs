using Model;
using Repository.CredentialsPersistance;
using Repository.NotificationsPersistance;
using Repository.PatientPersistance;
using Repository.PersonNotificationPersistance;
using System;
using System.Collections.Generic;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class NotificationService
    {
        private INotificationsRepository _notificationRepository;
        private IPersonNotificationRepository _personNotificationRepository;
        private ICredentialsRepository _credentialsRepository;

        public PatientGeneralService _patientGeneralService;
        public NotificationService(INotificationsRepository notificationsRepository, IPersonNotificationRepository personNotificationRepository, ICredentialsRepository credentialsRepository)
        {
            _notificationRepository = notificationsRepository;
            _personNotificationRepository = personNotificationRepository;
            _credentialsRepository = credentialsRepository;

            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            _patientGeneralService = new PatientGeneralService(patientRepository, credentialsRepository);
        }
        public List<Notification> GetAllNotifications()
        {
            return _notificationRepository.GetValues();
        }
        public List<PersonNotification> GetPersonNotifications()
        {
            return _personNotificationRepository.GetValues();
        }

        public void RemoveNotification(int id)
        {
            _notificationRepository.DeleteById(id);
            _personNotificationRepository.DeleteById(id);
        }

        private List<string> getRecipients(NotificationDTO notificationDTO)
        {
            List<string> recipients = new List<string>();
            List<Credentials> accounts = _credentialsRepository.GetValues();

            foreach (var account in accounts)
            {
                if ((account.Role.Equals(Model.RoleType.MANAGER) && notificationDTO.ManagerChecked)
                    || (account.Role.Equals(Model.RoleType.SECERATRY) && notificationDTO.SecretaryChecked)
                    || (account.Role.Equals(Model.RoleType.DOCTOR) && notificationDTO.DoctorChecked)
                    || (account.Role.Equals(Model.RoleType.PATIENT) && notificationDTO.PatientChecked))
                {
                    recipients.Add(account.Username);
                }
            }
            foreach (string username in notificationDTO.CustomRecipients)
            {
                if (!recipients.Contains(username))
                    recipients.Add(username);
            }
            return recipients;
        }

        public int CalculateNotificationId()
        {
            List<Notification> notifications = _notificationRepository.GetValues();
            if (notifications.Count == 0)
                return 1;
            else
                return notifications[notifications.Count - 1].NotificationId + 1;
        }

        public void ProcessNotificationSend(NotificationDTO notificationDTO)
        {
            int notificationId = CalculateNotificationId();
            createNotification(notificationDTO, notificationId);
            createPersonNotifications(notificationDTO, notificationId);
        }

        public void ProcessNotificationUpdate(NotificationDTO notificationDTO, int id)
        {
            _personNotificationRepository.DeleteById(id);
            _notificationRepository.DeleteById(id);
            ProcessNotificationSend(notificationDTO);
        }

        private void createNotification(NotificationDTO notificationDTO, int notificationId)
        {
            Notification newNotification = new Model.Notification(notificationDTO.NotificationText, DateTime.Now, "suki", notificationDTO.NotificationTitle, notificationId); // currently hardcoded, stored in static field viewmodel
            _notificationRepository.Create(newNotification);
        }
        private void createPersonNotifications(NotificationDTO notificationDTO, int notificationId)
        {
            List<string> recipients = getRecipients(notificationDTO);
            foreach (var recipient in recipients)
            {
                PersonNotification personNotification = new Model.PersonNotification(recipient, notificationId, false);
                _personNotificationRepository.Create(personNotification);
            }
        }

        public bool ProcessCustomRecipients(NotificationDTO notificationDTO, string customRecipient)
        {
            if (_patientGeneralService.IsPatientRegistered(customRecipient))
            {
                if (!notificationDTO.CustomRecipients.Contains(customRecipient))
                {
                    notificationDTO.CustomRecipients.Add(customRecipient);
                }
                return true;
            }
            return false;
        }
        public void RemoveCustomRecipient(NotificationDTO notificationDTO, string selectedRecipient)
        {
            notificationDTO.CustomRecipients.Remove(selectedRecipient);
        }

        public void CreateNewNotification(Notification notification)
        {
            _notificationRepository.Create(notification);
        }
        public void CreateNewPersonNotification(PersonNotification personNotification)
        {
            _personNotificationRepository.Create(personNotification);
        }
    }
}
