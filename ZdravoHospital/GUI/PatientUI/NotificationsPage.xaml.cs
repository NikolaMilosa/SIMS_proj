﻿using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {
        public ObservableCollection<NotificationView> NotificationList { get; set; }
        
        public string PatientUsername { get; set; }
        public NotificationsPage(string username)
        {
            InitializeComponent();
            DataContext = this;
            PatientUsername = username;
            fillList();
        }

        private void fillList()
        {
            NotificationList = new ObservableCollection<NotificationView>();
            Model.Resources.OpenNotifications();
            foreach(Notification notification in Model.Resources.notifications)
            {
                if (notification.UsernameRecievers.ContainsKey(PatientUsername))
                {
                    NotificationList.Add(new NotificationView(notification, PatientUsername));
                }
            }
        }

        private void notificationDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NotificationDetailsPage((NotificationView)notificationDataGrid.SelectedItem, PatientUsername));
        }
    }
}