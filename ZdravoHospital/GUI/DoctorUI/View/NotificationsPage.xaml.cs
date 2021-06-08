using Model;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.DTOs;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page, INotifyPropertyChanged
    {
        private NotificationService _notificationService;
        private List<NotificationDisplayDTO> _notificationDTOs;

        public Thickness ListViewPadding { get; set; }
        public double ListItemWidth { get; set; }
        private int _unreadCount;
        public int UnreadCount 
        {
            get => _unreadCount; 
            set
            {
                _unreadCount = value;
                OnPropertyChanged("UnreadCount");
            }
        }

        public NotificationsPage()
        {
            InitializeComponent();

            this.DataContext = this;

            _notificationService = new NotificationService();
            _notificationDTOs = _notificationService.GetNotificationDTOs(App.currentUser);
            NotificationsListView.ItemsSource = _notificationDTOs;

            foreach (NotificationDisplayDTO dto in _notificationDTOs)
                if (!dto.IsRead)
                    UnreadCount++;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged(string name)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            ListViewPadding = new Thickness(this.ActualWidth * 0.17, 0, this.ActualWidth * 0.17, 0);
            OnPropertyChanged("ListViewPadding");
            ListItemWidth = NotificationsListView.ActualWidth;
            OnPropertyChanged("ListItemWidth");
        }

        private void MarkAsReadButton_Click(object sender, RoutedEventArgs e)
        {
            NotificationDisplayDTO dto = (sender as Button).DataContext as NotificationDisplayDTO;
            dto.IsRead = true;
        }
    }
}
