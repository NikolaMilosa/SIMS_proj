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
using Model;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryNewPeriodPage.xaml
    /// </summary>
    public partial class SecretaryNewPeriodPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }
        private int _periodTypeIndex;
        private string _time;
        private string _duration;
        private DateTime _date;
        private Doctor _doctor;
        private Patient _patient;
        private Room _room;
        public PeriodAvailability PeriodAvailable { get; set; }

        public enum PeriodAvailability
        {
            AVAILABLE,
            DOCTOR_UNAVAILABLE,
            PATIENT_UNAVAILABLE,
            ROOM_UNAVAILABLE,
            TIME_UNACCEPTABLE
        }

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
        public SecretaryNewPeriodPage()
        {
            InitializeComponent();
            this.DataContext = this;


            Model.Resources.DeserializeDoctors();

            Model.Resources.OpenPatients();

            Model.Resources.OpenRooms();
            Model.Resources.OpenPeriods();

            initializeListsForBinding();

            setSearchFilters();
        }

        private void initializeListsForBinding()
        {
            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
        }

        private void setDoctorFilter()
        {
            ICollectionView viewDoctors = (ICollectionView)CollectionViewSource.GetDefaultView(Doctors);
            viewDoctors.Filter = DoctorsFilter;
        }
        private void setPatientFilter()
        {
            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(Patients);
            viewPatients.Filter = PatientsFilter;
        }
        private void setRoomFilter()
        {
            ICollectionView viewRooms = (ICollectionView)CollectionViewSource.GetDefaultView(Rooms);
            viewRooms.Filter = RoomsFilter;
        }
        private void setSearchFilters()
        {
            setDoctorFilter();
            setPatientFilter();
            setRoomFilter();
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool DoctorsFilter(object item)
        {
            if (String.IsNullOrEmpty(DoctorTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(DoctorTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool PatientsFilter(object item)
        {
            if (String.IsNullOrEmpty(PatientTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(PatientTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool RoomsFilter(object item)
        {
            if (String.IsNullOrEmpty(RoomTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(RoomTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private void RoomsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(RoomsListBox.ItemsSource).Refresh();
        }

        private void DoctorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(DoctorsListBox.ItemsSource).Refresh();
        }

        private void PatientsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PatientsListBox.ItemsSource).Refresh();
        }

        private void checkPeriodAvailability(Period period)
        {
            setInitialPeriodAvailability();
            checkTimeAvailabilityForPeriod(period);
            checkDoctorAvailabilityForPeriod(period);
            checkPatientAvailabilityForPeriod(period);
            checkRoomAvailabilityForPeriod(period);
        }

        private void setInitialPeriodAvailability()
        {
            this.PeriodAvailable = PeriodAvailability.AVAILABLE;
        }

        private void checkTimeAvailabilityForPeriod(Period period)
        {
            if (period.StartTime < DateTime.Now.AddMinutes(15))
            {
                this.PeriodAvailable = PeriodAvailability.TIME_UNACCEPTABLE;
            }
        }

        private void checkDoctorAvailabilityForPeriod(Period period)
        {
            foreach (Period existingPeriod in Model.Resources.periods)
            {
                if (periodsHaveSameDoctors(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    this.PeriodAvailable = PeriodAvailability.DOCTOR_UNAVAILABLE;
                }
            }
        }
        private void checkPatientAvailabilityForPeriod(Period period)
        {
            foreach (Period existingPeriod in Model.Resources.periods)
            {
                if (periodsHaveSamePatients(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    this.PeriodAvailable = PeriodAvailability.PATIENT_UNAVAILABLE;
                }
            }
        }

        private void checkRoomAvailabilityForPeriod(Period period)
        {
            foreach (Period existingPeriod in Model.Resources.periods)
            {
                if (periodsHaveSameRooms(period, existingPeriod) && periodsOverlap(period, existingPeriod))
                {
                    this.PeriodAvailable = PeriodAvailability.ROOM_UNAVAILABLE;
                }
            }
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

        private bool periodsHaveSameDoctors(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.DoctorUsername == existingPeriod.DoctorUsername)
            {
                return true;
            }
            return false;
        }

        private bool periodsHaveSamePatients(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.PatientUsername == existingPeriod.PatientUsername)
            {
                return true;
            }
            return false;
        }

        private bool periodsHaveSameRooms(Period newPeriod, Period existingPeriod)
        {
            if (newPeriod.RoomId == existingPeriod.RoomId)
            {
                return true;
            }
            return false;
        }

        private Period createPeriodWithInputFields()
        {
            string[] hoursAndMinutes = Time.Split(":");
            DateTime periodStartTime = new DateTime(Date.Year, Date.Month, Date.Day, Int32.Parse(hoursAndMinutes[0]), Int32.Parse(hoursAndMinutes[1]), 0);
            Period newPeriod = new Period(periodStartTime, Int32.Parse(Duration), (PeriodType)PeriodTypeIndex, Patient.Username, Doctor.Username, Room.Id);
            return newPeriod;
        }

        private void savePeriod(Period period)
        {
            Model.Resources.OpenPeriods();
            Model.Resources.periods.Add(period);
            Model.Resources.SavePeriods();
        }

        private bool isPeriodAvailable()
        {
            if (this.PeriodAvailable == PeriodAvailability.AVAILABLE)
            {
                return true;
            }
            return false;
        }

        private void giveAvailabilityFeedbackMessage()
        {
            if (this.PeriodAvailable == PeriodAvailability.DOCTOR_UNAVAILABLE)
                MessageBox.Show("Selected doctor is unavailable in selected period.", "Doctor unavailable");
            else if (this.PeriodAvailable == PeriodAvailability.PATIENT_UNAVAILABLE)
                MessageBox.Show("Selected patient is unavailable in selected period.", "Patient unavailable");
            else if (this.PeriodAvailable == PeriodAvailability.ROOM_UNAVAILABLE)
                MessageBox.Show("Selected room is unavailable in selected period.", "Room unavailable");
            else
                MessageBox.Show("Selected time is not acceptable.", "Time unacceptable");
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            Period period = createPeriodWithInputFields();
            checkPeriodAvailability(period);

            if (isPeriodAvailable())
            {
                savePeriod(period);
                NavigationService.Navigate(new SecretaryPeriodsPage());
            }
            else
            {
                giveAvailabilityFeedbackMessage();
            }
        }
    }
}
