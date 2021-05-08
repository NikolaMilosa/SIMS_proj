﻿using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryUrgentPeriodPage.xaml
    /// </summary>
    public partial class SecretaryUrgentPeriodPage : Page
    {
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }

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
        public SecretaryUrgentPeriodPage()
        {
            InitializeComponent();
            this.DataContext = this;

            Model.Resources.OpenPatients();
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);

            Model.Resources.OpenSpecializations();
            Specializations = new ObservableCollection<Specialization>(Model.Resources.specializations);

            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(Patients);
            viewPatients.Filter = UserFilterPatients;
        }
        private bool UserFilterPatients(object item)
        {
            if (String.IsNullOrEmpty(PatientTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(PatientTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void PatientTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PatientsListBox.ItemsSource).Refresh();
        }

        private void GuestAccountButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new GuestAccountPage(true));
        }

        private void CreatePeriod_Click(object sender, RoutedEventArgs e)
        {
            List<Period> freePeriods = this.findFreePeriods();
            if(freePeriods.Count == 0)
            {
                //Logika za kad nema slobodnih termina
                List<Doctor> doctors = this.findDoctorsBySpecialization(SelectedSpecialization);
                List<Period> periods = new List<Period>();

                foreach(var doctor in doctors)
                {
                    List<Period> doctorPeriods = this.findPeriodsByDoctor(doctor.Username);
                    List<Period> periodsWithinRange = this.findPeriodsWithinRange(doctorPeriods, DateTime.Now, 60 + Int32.Parse(Duration));
                    List<Period> tryoutPeriods = this.findTryoutPeriods(periodsWithinRange);
                    this.setMovePeriods(tryoutPeriods, periodsWithinRange);
                    foreach(var tryout in tryoutPeriods){
                        if(tryout.MovePeriods.Count != 0)
                            periods.Add(tryout);
                    }
                }

                if(periods.Count == 0)
                    MessageBox.Show("Sorry, no doctors available.");
                else
                    NavigationService.Navigate(new PeriodsToMovePage(periods));
            }
            else
            {
                Period bestPeriod = this.findBestPeriod(freePeriods);
                Model.Resources.OpenPeriods();

                //set room
                List<Room> availableEmergencyRooms = findAvailableEmergencyRooms(bestPeriod);
                if (availableEmergencyRooms.Count != 0)
                    bestPeriod.RoomId = availableEmergencyRooms[0].Id;

                Model.Resources.periods.Add(bestPeriod);
                Model.Resources.SavePeriods();
                NavigationService.Navigate(new UrgentPeriodSummaryPage(bestPeriod));
            }
            
        }

        private List<Period> findFreePeriods()
        {
            List<Doctor> doctors = this.findDoctorsBySpecialization(SelectedSpecialization);
            
            List<Period> freePeriods = new List<Period>();
            int duration = Int32.Parse(Duration);

            foreach (Doctor doctor in doctors)
            {
                DateTime startCheckTime = DateTime.Now;
                DateTime endCheckTime = startCheckTime.AddMinutes(60);
                while (startCheckTime < endCheckTime)
                {
                    Period tryPeriod = new Period(startCheckTime, duration, Patient.Username, doctor.Username, true);
                    if (this.IsPeriodAvailable(tryPeriod))
                    {
                        freePeriods.Add(tryPeriod);
                        break;
                    }

                    startCheckTime = startCheckTime.AddMinutes(1);
                }
            }

            return freePeriods;
        }


        private List<Doctor> findDoctorsBySpecialization(Specialization specialization)
        {
            List<Doctor> doctors = new List<Doctor>();
            Model.Resources.OpenDoctors();
            foreach (KeyValuePair<string, Doctor> item in Model.Resources.doctors)
            {
                if (item.Value.SpecialistType.SpecializationName.Equals(specialization.SpecializationName))
                {
                    doctors.Add(item.Value);
                }
            }
            return doctors;
        }

        private bool IsPeriodAvailable(Period period) // vraca 0 ako je termin ok, 1 ako je soba zauzeta, 2 ako je doktor zauzet, 3 ako je pacijent zauzet
        {

            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
            Model.Resources.OpenPeriods();

            foreach (Period existingPeriod in Model.Resources.periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime < existingPeriodEndTime && periodEndtime > existingPeriod.StartTime)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
        
        private Period findBestPeriod(List<Period> periods)
        {
            Period bestPeriod = periods[0];
            foreach(var period in periods)
            {
                if(period.StartTime < bestPeriod.StartTime)
                {
                    bestPeriod = period;
                }
            }
            return bestPeriod;
        }

        private List<Period> findPeriodsWithinRange(List<Period> periods, DateTime start, int duration)
        {
            List<Period> periodsWithinRange = new List<Period>();
            DateTime existingPeriodEndTime = start.AddMinutes(duration);
            foreach (var period in periods)
            {
                DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
                if (period.StartTime < existingPeriodEndTime && periodEndtime > start)
                {
                    periodsWithinRange.Add(period);
                }
            }
            return periodsWithinRange;
        }

        private List<Period> findPeriodsByDoctor(string doctorUsername)
        {
            List<Period> doctorPeriods = new List<Period>();
            Model.Resources.OpenPeriods();
            foreach(var period in Model.Resources.periods)
            {
                if(period.DoctorUsername == doctorUsername)
                {
                    doctorPeriods.Add(period);
                }
            }
            return doctorPeriods;
        }

        private List<Period> findTryoutPeriods(List<Period> periodsWithinRange)
        {
            // creating periods now and on the end of every existing period within range [0, 1h + duration] if point is within [0, +1h]
            List<DateTime> timePoints = new List<DateTime>();
            List<Period> tryoutPeriods = new List<Period>();
            string doctorUsername = periodsWithinRange[0].DoctorUsername;
            if(!isTimePointWithinAnyPeriod(DateTime.Now, periodsWithinRange))
                timePoints.Add(DateTime.Now);
            foreach(var period in periodsWithinRange)
            {
                if(period.StartTime.AddMinutes(period.Duration) < DateTime.Now.AddMinutes(60))
                {
                    timePoints.Add(period.StartTime.AddMinutes(period.Duration));
                }
            }
            foreach(var tp in timePoints)
            {
                Period tryPeriod = new Period(tp, Int32.Parse(Duration), Patient.Username, doctorUsername, true);
                //check if tryperiod overlaps urgent period
                List<Period> urgentPeriods = this.findUrgentPeriods(periodsWithinRange);
                int periodsOverlappingUrgent = this.findPeriodsWithinRange(urgentPeriods, tryPeriod.StartTime, tryPeriod.Duration).Count;
                if(periodsOverlappingUrgent == 0)
                    tryoutPeriods.Add(tryPeriod);
            }
            return tryoutPeriods;
        }

        private List<Period> findUrgentPeriods(List<Period> periods)
        {
            List<Period> urgentPeriods = new List<Period>();
            foreach (var period in periods)
            {
                if (period.IsUrgent)
                    urgentPeriods.Add(period);
            }
            return urgentPeriods;
        }

        private bool isTimePointWithinPeriod(DateTime point, Period period)
        {
            if (point > period.StartTime && point < period.StartTime.AddMinutes(period.Duration))
                return true;
            else
                return false;
        }

        private bool isTimePointWithinAnyPeriod(DateTime point, List<Period> periods)
        {
            foreach(Period period in periods)
            {
                if(isTimePointWithinPeriod(point, period))
                {
                    return true;
                }
            }
            return false;
        }

        private void setMovePeriods(List<Period> tryoutPeriods, List<Period> periodsWithinRange)
        {
            foreach(var tryPeriod in tryoutPeriods)
            {
                List<Period> overlappingPeriods = this.findPeriodsWithinRange(periodsWithinRange, tryPeriod.StartTime, tryPeriod.Duration);
                overlappingPeriods.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));
                foreach(var overlap in overlappingPeriods)
                {
                    DateTime initialStartTime = overlap.StartTime;
                    DateTime newStartTime = findFreeStartTime(overlap, tryPeriod.MovePeriods, tryPeriod.StartTime.AddMinutes(tryPeriod.Duration), overlappingPeriods);
                    MovePeriod movePeriod = new MovePeriod(overlap.DoctorUsername, overlap.PatientUsername, overlap.RoomId, initialStartTime, newStartTime, overlap.Duration);
                    tryPeriod.MovePeriods.Add(movePeriod);
                }
            }
        }

        private DateTime findFreeStartTime(Period period, ObservableCollection<MovePeriod> movePeriods, DateTime urgentEndTime, List<Period> overlappingPeriods)
        {
            List<Period> overlappingPeriodsFinal = this.cloneListOfPeriods(overlappingPeriods);
            DateTime initialStartTime = period.StartTime;
            period.StartTime = urgentEndTime;
            
            while (!IsPeriodAvailableIncludingMovePeriods(period, movePeriods, overlappingPeriodsFinal))
            {
                period.StartTime = period.StartTime.AddMinutes(1);
            }
            DateTime newStartTime = period.StartTime;
            period.StartTime = initialStartTime;

            return newStartTime;
        }

        private bool IsPeriodAvailableIncludingMovePeriods(Period period, ObservableCollection<MovePeriod> movePeriods, List<Period> overlappingPeriods)
        {

            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);
            Model.Resources.OpenPeriods();

            foreach (Period existingPeriod in Model.Resources.periods)
            {
                bool isOverlaping = false;
                foreach(var overlap in overlappingPeriods)
                {
                    if (existingPeriod.StartTime == overlap.StartTime && existingPeriod.RoomId == overlap.RoomId)
                        isOverlaping = true;
                }

                if (isOverlaping)
                    continue;

                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime < existingPeriodEndTime && periodEndtime > existingPeriod.StartTime)
                    {
                        return false;
                    }
                }
            }

            foreach(MovePeriod mp in movePeriods)
            {
                DateTime existingMovePeriodEndTime = mp.MovedStartTime.AddMinutes(mp.Duration);
                if (period.StartTime < existingMovePeriodEndTime && periodEndtime > mp.MovedStartTime)
                {
                    return false;
                }
            }

            return true;
        }

        private List<Period> cloneListOfPeriods(List<Period> periods)
        {
            List<Period> newList = new List<Period>();
            foreach(var period in periods)
            {
                Period newPeriod = new Period();
                newPeriod.StartTime = period.StartTime;
                newPeriod.RoomId = period.RoomId;
                newPeriod.Duration = period.Duration;
                newList.Add(newPeriod);
            }
            return newList;
        }

        private List<Model.Room> findAvailableEmergencyRooms(Period newPeriod)
        {
            Model.Resources.OpenRooms();
            List<Model.Room> availableRooms = new List<Room>();
            foreach (KeyValuePair<int, Model.Room> item in Model.Resources.rooms)
            {
                if (item.Value.RoomType == RoomType.EMERGENCY_ROOM)
                {
                    bool available = true;
                    foreach (Period existingPeriod in Model.Resources.periods)
                    {
                        if (periodsOverlap(newPeriod, existingPeriod))
                        {
                            if (item.Key == existingPeriod.RoomId)
                                available = false;
                        }
                    }
                    if (available)
                        availableRooms.Add(item.Value);
                }

            }
            return availableRooms;
        }

        private bool periodsOverlap(Period newPeriod, Period existingPeriod)
        {
            DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);
            DateTime newPeriodEndtime = newPeriod.StartTime.AddMinutes(newPeriod.Duration);
            if (newPeriod.StartTime < existingPeriodEndTime && newPeriodEndtime > existingPeriod.StartTime)
            {
                return true;
            }
            return false;
        }
    }
}