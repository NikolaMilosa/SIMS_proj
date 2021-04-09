using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        public DoctorWindow()
        {
            InitializeComponent();

            Model.Resources.DeserializeDoctors();
            Doctor doctor = Model.Resources.doctors[App.currentUser];
            UserTextBlock.Text = doctor.Name + " " + doctor.Surname;
        }

        private void ScheduleTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (PatientsTab.IsSelected)
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dark_gray.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dark_gray.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (NotificationsTab.IsSelected)
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dark_gray.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.DarkGray;
            }

            ScheduleTab.IsSelected = true;
            ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dodger_blue.png", UriKind.Relative));
            ScheduleTabTextBlock.Foreground = Brushes.DodgerBlue;
        }

        private void ScheduleTabButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!ScheduleTab.IsSelected)
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_white.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.White;
            }
        }

        private void ScheduleTabButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (ScheduleTab.IsSelected)
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dodger_blue.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.DodgerBlue;
            }
            else
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dark_gray.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.DarkGray;
            }
        }

        private void PatientsTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleTab.IsSelected)
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dark_gray.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dark_gray.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (NotificationsTab.IsSelected)
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dark_gray.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.DarkGray;
            }

            PatientsTab.IsSelected = true;
            PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dodger_blue.png", UriKind.Relative));
            PatientsTabTextBlock.Foreground = Brushes.DodgerBlue;
        }

        private void PatientsTabButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!PatientsTab.IsSelected)
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_white.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.White;
            }
        }

        private void PatientsTabButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (PatientsTab.IsSelected)
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dodger_blue.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.DodgerBlue;
            }
            else
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dark_gray.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.DarkGray;
            }
        }

        private void MedicinesTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleTab.IsSelected)
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dark_gray.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (PatientsTab.IsSelected)
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dark_gray.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (NotificationsTab.IsSelected)
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dark_gray.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.DarkGray;
            }

            MedicinesTab.IsSelected = true;
            MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dodger_blue.png", UriKind.Relative));
            MedicinesTabTextBlock.Foreground = Brushes.DodgerBlue;
        }

        private void MedicinesTabButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_white.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.White;
            }
        }

        private void MedicinesTabButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dodger_blue.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.DodgerBlue;
            }
            else
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dark_gray.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.DarkGray;
            }
        }
        
        private void NotificationsTabButton_Click(object sender, RoutedEventArgs e)
        {
            if (ScheduleTab.IsSelected)
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_dark_gray.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (PatientsTab.IsSelected)
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_dark_gray.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Brushes.DarkGray;
            }
            else if (MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_dark_gray.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.DarkGray;
            }

            NotificationsTab.IsSelected = true;
            NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dodger_blue.png", UriKind.Relative));
            NotificationsTabTextBlock.Foreground = Brushes.DodgerBlue;
        }

        private void NotificationsTabButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!NotificationsTab.IsSelected)
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_white.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.White;
            }
        }

        private void NotificationsTabButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (NotificationsTab.IsSelected)
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dodger_blue.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.DodgerBlue;
            }
            else
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_dark_gray.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Brushes.DarkGray;
            }
        }
    }
}
