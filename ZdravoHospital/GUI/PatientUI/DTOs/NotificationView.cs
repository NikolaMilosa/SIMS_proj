using Model;
using Model.Repository;
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
            NotificationRepository notificationRepository = new NotificationRepository();
            
            foreach(Notification notification in notificationRepository.GetValues())
            {
                if (notification.NotificationId == personNotification.NotificationId)
                {
                    Notification = notification;
                    break;
                }
            }
            Seen = personNotification.IsRead;
            RoleType role = GetRoleType(username);
            switch(role)
            {
                case RoleType.DOCTOR:
                    Doctor doctor = GetDoctor(username);
                    From = role.ToString() + " " + doctor.Name + " " + doctor.Surname;
                    break;

                case RoleType.SECERATRY:
                    From = "Secretary Srdjan Sukovic";
                    break;
                default:
                    From = "Manager Nikola Milosavljevic";
                    break;
            }
        }

        private RoleType GetRoleType(string username)
        {
            AccountRepository accountRepository = new AccountRepository();
            return accountRepository.GetById(username).Role;
        }
        private Doctor GetDoctor(string username)
        {
            DoctorRepository doctorRepository = new DoctorRepository();
            return doctorRepository.GetById(username);
        }

    }
}
