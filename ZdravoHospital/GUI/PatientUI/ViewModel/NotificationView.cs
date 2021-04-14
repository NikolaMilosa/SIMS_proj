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

        public NotificationView(Notification notification,string username)
        {
            Notification = notification;
            Seen = notification.UsernameRecievers[username];
            Model.Resources.OpenAccounts();
            Model.RoleType role = Resources.accounts[Notification.UsernameSender].Role;
            switch(role)
            {
                case RoleType.DOCTOR:
                    Resources.DeserializeDoctors();
                    From = role.ToString() + " " + Resources.doctors[Notification.UsernameSender].Name + " " + Resources.doctors[Notification.UsernameSender].Surname;
                    break;

                default:
                    From = "Secretary Srdjan Sukovic";
                    break;

            }
        }

    }
}
