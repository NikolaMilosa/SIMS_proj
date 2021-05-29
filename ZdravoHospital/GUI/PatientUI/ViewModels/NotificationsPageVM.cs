using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Controls;
using System.Windows.Navigation;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Converters;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class NotificationsPageVM
    {
        #region Properties
        public ObservableCollection<NotificationDTO> Notifications { get; set; }
        public NotificationDTO SelectedNotification { get; set; }
        #endregion
        #region Constructors

        public NotificationsPageVM()
        {
            FillList();
            DoubleClickCommand = new RelayCommand(DoubleClickExecute);
        }

        #endregion
        #region Commands

        public RelayCommand DoubleClickCommand { get; private set; }

        #endregion
        #region CommandActions

        public void DoubleClickExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new NotificationDetailsPage(SelectedNotification));
        }

        #endregion
        #region Methods
        private void FillList()
        {
            Notifications = new ObservableCollection<NotificationDTO>();
            PersonNotificationRepository personNotificationRepository = new PersonNotificationRepository();
            NotificationConverter notificationConverter = new NotificationConverter();
            foreach (PersonNotification personNotification in personNotificationRepository.GetValues())
                if (personNotification.Username.Equals(PatientWindowVM.PatientUsername))
                    Notifications.Add(notificationConverter.GetNotifcationDTO(personNotification));
                
            
        }

        #endregion

    }
}
