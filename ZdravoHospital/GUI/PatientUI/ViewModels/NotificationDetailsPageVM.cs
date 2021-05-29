using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class NotificationDetailsPageVM
    {
        #region Properties
        public NotificationDTO NotificationDTO { get; set; }
        public NotificationFunctions NotificationFunctions { get; private set; }
        #endregion

        #region Constructor

        public NotificationDetailsPageVM(NotificationDTO notificationDto)
        {
            NotificationDTO = notificationDto;
            NotificationFunctions = new NotificationFunctions();
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
            NotificationFunctions.UpdatePersonNotification(GetPersonNotification());
        }

        private PersonNotification GetPersonNotification()
        {
            List<PersonNotification> personNotifications = NotificationFunctions.GetPersonNotifications();
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
