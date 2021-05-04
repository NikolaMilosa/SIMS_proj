using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

using Model;

namespace ZdravoHospital.GUI.ManagerUI.DTOs
{
    public class ReservationDTO : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;
        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private ReservationType _reservationType;
        private DateTime _start;
        private DateTime _end;

        public ReservationType ReservationType
        {
            get => _reservationType;
            set
            {
                _reservationType = value;
                OnPropertyChanged("ReservationType");
            }
        }

        public DateTime Start
        {
            get => _start;
            set
            {
                _start = value;
                OnPropertyChanged("Start");
            }
        }

        public DateTime End
        {
            get => _end;
            set
            {
                _end = value;
                OnPropertyChanged("End");
            }
        }

        public ReservationDTO(ReservationType rt, DateTime s, DateTime e)
        {
            ReservationType = rt;
            Start = s;
            End = e;
        }
    }
}
