using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public class GuestDTO
    {
        public GuestDTO()
        {

        }
        public GuestDTO(bool urgentlyCreated)
        {
            UrgentlyCreated = urgentlyCreated;
        }

        public GuestDTO(string name, string surname, string citizenId, string healthCardNumber)
        {
            Name = name;
            Surname = surname;
            CitizenId = citizenId;
            HealthCardNumber = healthCardNumber;
        }

        public bool UrgentlyCreated { get; set; }
        public string Username { get; set; }
        private string _name;
        private string _surname;
        private string _citizenId;
        private string _healthCardNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("PName");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }
        public string CitizenId
        {
            get => _citizenId;
            set
            {
                _citizenId = value;
                OnPropertyChanged("CitizenId");
            }
        }
        public string HealthCardNumber
        {
            get => _healthCardNumber;
            set
            {
                _healthCardNumber = value;
                OnPropertyChanged("HealthCardNumber");
            }
        }
    }
}
