using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.DTOs
{
    public class NotificationDisplayDTO
    {
        public Notification Notification { get; set; }
        public bool IsRead { get; set; }
        public double ButtonWidth
        {
            get => IsRead ? 0 : 180;
        }

        public NotificationDisplayDTO(Notification notification, bool isRead)
        {
            Notification = notification;
            IsRead = isRead;
        }
    }
}
