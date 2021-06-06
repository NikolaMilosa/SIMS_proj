using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for DoctorWindow.xaml
    /// </summary>
    public partial class DoctorWindow : Window
    {
        private MediaElement _mediaElement;

        public DoctorWindow()
        {
            InitializeComponent();

            _mediaElement = TutorialMediaElement;
            DataContext = new DoctorViewModel(_mediaElement);
        }

        /*
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
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_blue.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Application.Current.Resources["Blue"] as Brush;
            }
            else
            {
                ScheduleTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/schedule_light_gray.png", UriKind.Relative));
                ScheduleTabTextBlock.Foreground = Application.Current.Resources["LightGray"] as Brush;
            }
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
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_blue.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Application.Current.Resources["Blue"] as Brush;
            }
            else
            {
                PatientsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/patients_light_gray.png", UriKind.Relative));
                PatientsTabTextBlock.Foreground = Application.Current.Resources["LightGray"] as Brush;
            }
        }

        private void MedicinesTabButton_MouseEnter(object sender, MouseEventArgs e)
        {
            if (!MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri(".//../../Images/Doctor/medicines_white.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Brushes.White;
            }
        }

        private void MedicinesTabButton_MouseLeave(object sender, MouseEventArgs e)
        {
            if (MedicinesTab.IsSelected)
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_blue.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Application.Current.Resources["Blue"] as Brush;
            }
            else
            {
                MedicinesTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/medicines_light_gray.png", UriKind.Relative));
                MedicinesTabTextBlock.Foreground = Application.Current.Resources["LightGray"] as Brush;
            }
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
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_blue.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Application.Current.Resources["Blue"] as Brush;
            }
            else
            {
                NotificationsTabImage.Source = new BitmapImage(new Uri("../../Images/Doctor/notifications_light_gray.png", UriKind.Relative));
                NotificationsTabTextBlock.Foreground = Application.Current.Resources["LightGray"] as Brush;
            }
        }
        */

    }
}
