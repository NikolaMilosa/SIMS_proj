using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTOs
{
    public enum UrgentPeriodStatus
    {
        FREE_TO_BOOK,
        NO_DOCTORS_AVAILABLE,
        PERIODS_TO_MOVE
    }
    public class UrgentPeriodDTO
    {
        private Specialization _selectedSpecialization;
        public Specialization SelectedSpecialization
        {
            get { return _selectedSpecialization; }
            set
            {
                _selectedSpecialization = value;
                OnPropertyChanged("SelectedSpecialization");
            }
        }
        private Patient _patient;
        public Patient Patient
        {
            get { return _patient; }
            set
            {
                _patient = value;
                OnPropertyChanged("Patient");
            }
        }
        private string _duration;
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
