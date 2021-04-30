using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;

namespace ZdravoHospital.GUI.ManagerUI.DTOs
{
    public class RoomScheduleDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private DateTime _date;
        private ObservableCollection<ReservationDTO> _reservations; 

        public DateTime Date
        {
            get => _date;
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public ObservableCollection<ReservationDTO> Reservations
        {
            get => _reservations;
            set
            {
                _reservations = value;
                OnPropertyChanged("Reservations");
            }
        }

        public RoomScheduleDTO(DateTime d)
        {
            Date = d;
            Reservations = new ObservableCollection<ReservationDTO>();
        }
    }
}
