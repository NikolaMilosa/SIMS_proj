using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.PatientUI.ViewModel
{
    public class NotificationView
    {
        public Notification Notification { get; set; }
        public string From { get; set; }//Role Name Surname

        public bool Seen { get; set; }

        public NotificationView(PersonNotification personNotification,string username)
        {
            Resources.OpenNotifications();
            foreach(Notification notification in Resources.notifications)
            {
                if (notification.NotificationId == personNotification.NotificationId)
                {
                    Notification = notification;
                    break;
                }
            }
            Seen = personNotification.IsRead;
            Model.Resources.OpenAccounts();
            Model.RoleType role = Resources.accounts[Notification.UsernameSender].Role;
            switch(role)
            {
                case RoleType.DOCTOR:
                    Resources.DeserializeDoctors();
                    From = role.ToString() + " " + Resources.doctors[Notification.UsernameSender].Name + " " + Resources.doctors[Notification.UsernameSender].Surname;
                    break;

                case RoleType.SECERATRY:
                    From = "Secretary Srdjan Sukovic";
                    break;
                default:
                    From = "Manager Nikola Milosavljevic";
                    break;
            }
        }

    }
}
