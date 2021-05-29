using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public enum PeriodAvailability
    {
        AVAILABLE,
        DOCTOR_UNAVAILABLE,
        PATIENT_UNAVAILABLE,
        ROOM_UNAVAILABLE,
        TIME_UNACCEPTABLE
    }
    public class PeriodDTO
    {
        public static int MIN_MINUTES_DIFFERENCE = 15;
        private int _periodTypeIndex;
        private string _time;
        private string _duration;
        private DateTime _date;
        private Doctor _doctor;
        private Patient _patient;
        private Room _room;
        public PeriodAvailability PeriodAvailable { get; set; }
        public Room Room
        {
            get { return _room; }
            set
            {
                _room = value;
                OnPropertyChanged("Room");
            }
        }

        public Doctor Doctor
        {
            get { return _doctor; }
            set
            {
                _doctor = value;
                OnPropertyChanged("Doctor");
            }
        }
        public Patient Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged("Patient");
            }
        }

        public DateTime Date
        {
            get { return _date; }
            set
            {
                _date = value;
                OnPropertyChanged("Date");
            }
        }

        public int PeriodTypeIndex
        {
            get { return _periodTypeIndex; }
            set
            {
                _periodTypeIndex = value;
                OnPropertyChanged("PeriodTypeIndex");
            }
        }
        public string Time
        {
            get { return _time; }
            set
            {
                _time = value;
                OnPropertyChanged("Time");
            }
        }
        public string Duration
        {
            get { return _duration; }
            set
            {
                _duration = value;
                OnPropertyChanged("Duration");
            }
        }

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
