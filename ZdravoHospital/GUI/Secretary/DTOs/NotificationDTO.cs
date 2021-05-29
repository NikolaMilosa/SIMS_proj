using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class NotificationDTO
    {
        public NotificationDTO()
        {
            Recipients = new List<string>();
            CustomRecipients = new ObservableCollection<string>();
        }
        private string _notificationTitle;
        private string _notificationText;
        private bool _managerChecked;
        private bool _secretaryChecked;
        private bool _doctorChecked;
        private bool _patientChecked;

        public string NotificationTitle
        {
            get => _notificationTitle;
            set
            {
                _notificationTitle = value;
                OnPropertyChanged("NotificationTitle");
            }
        }
        public string NotificationText
        {
            get => _notificationText;
            set
            {
                _notificationText = value;
                OnPropertyChanged("NotificationText");
            }
        }

        public bool ManagerChecked
        {
            get => _managerChecked;
            set
            {
                _managerChecked = value;
                OnPropertyChanged("ManagerChecked");
            }
        }
        public bool SecretaryChecked
        {
            get => _secretaryChecked;
            set
            {
                _secretaryChecked = value;
                OnPropertyChanged("SecretaryChecked");
            }
        }
        public bool DoctorChecked
        {
            get => _doctorChecked;
            set
            {
                _doctorChecked = value;
                OnPropertyChanged("DoctorChecked");
            }
        }
        public bool PatientChecked
        {
            get => _patientChecked;
            set
            {
                _patientChecked = value;
                OnPropertyChanged("PatientChecked");
            }
        }
        public List<string> Recipients { get; set; }
        public ObservableCollection<string> CustomRecipients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
