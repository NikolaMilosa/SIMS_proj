using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Repository.CredentialsPersistance;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;

namespace ZdravoHospital.GUI.PatientUI.Converters
{
    public class NotificationConverter
    {
        public NotificationDTO GetNotifcationDTO(PersonNotification personNotification)
        {
            Notification notification = GetNotification(personNotification);
            string from = GetSender(notification.UsernameSender);
            return new NotificationDTO(personNotification.Username,personNotification.NotificationId ,notification.CreateDate, from,
                personNotification.IsRead,notification.Title,notification.Text);
        }

        public PersonNotification GetPersonNotification(NotificationDTO perNotificationDTO)
        {
            return null;
        }

        private string GetSender(string username)
        {
            RoleType role = GetRoleType(username);
            switch (role)
            {
                case RoleType.DOCTOR:
                    Doctor doctor = GetDoctor(username);
                    return role.ToString() + " " + doctor.Name + " " + doctor.Surname;
                case RoleType.SECERATRY:
                    return "Secretary Srdjan Sukovic";
                default:
                    return "Manager Nikola Milosavljevic";

            }
        }
    

        private RoleType GetRoleType(string username)
        {
            CredentialsRepository credentialsRepository = new CredentialsRepository();
            return credentialsRepository.GetById(username).Role;
        }

        private Doctor GetDoctor(string username)
        {
            DoctorFunctions doctorFunctions = new DoctorFunctions();
            return doctorFunctions.GetDoctor(username);
        }

        private Notification GetNotification(PersonNotification personNotification)
        {
            NotificationFunctions notificationFunctions = new NotificationFunctions();
            var notifications = notificationFunctions.GetNotifications();
            return notifications.FirstOrDefault(notification => notification.NotificationId.Equals(personNotification.NotificationId));
        }

    }
}
