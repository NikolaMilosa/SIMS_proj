using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class NotificationDetailsPageVM
    {
        #region Properties
        public NotificationDTO NotificationDTO { get; set; }


        #endregion

        #region Constructor

        public NotificationDetailsPageVM(NotificationDTO notificationDto)
        {
            NotificationDTO = notificationDto;
            SerializeReadNotification();
            BackCommand = new RelayCommand(BackExecute);
        }

        #endregion

        #region Commands

        public RelayCommand BackCommand { get; private set; }

        #endregion

        #region CommandActions
        public void BackExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new NotificationsPage(PatientWindowVM.PatientUsername));
        }
        #endregion

        #region Methods
        private void SerializeReadNotification()
        {

            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            personNotificationRepository.Update(GetPersonNotification());
        }

        private PersonNotification GetPersonNotification()
        {
            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            List<PersonNotification> personNotifications = personNotificationRepository.GetValues();
            foreach (var personNotification in personNotifications.Where(personNotification => personNotification.NotificationId.Equals(NotificationDTO.Id) &&
                personNotification.Username.Equals(NotificationDTO.Username)))
            {
                personNotification.IsRead = true;
                return personNotification;
            }

            return null;
        }

        #endregion
    }
}
