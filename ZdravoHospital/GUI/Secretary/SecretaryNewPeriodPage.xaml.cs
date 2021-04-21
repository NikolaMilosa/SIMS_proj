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

            if(Model.Resources.doctors == null)
                Model.Resources.DeserializeDoctors();
            if(Model.Resources.patients == null)
                Model.Resources.OpenPatients();
            if(Model.Resources.rooms == null)
                Model.Resources.OpenRooms();

            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);

            ICollectionView viewDoctors = (ICollectionView)CollectionViewSource.GetDefaultView(Doctors);
            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(Patients);
            ICollectionView viewRooms = (ICollectionView)CollectionViewSource.GetDefaultView(Rooms);

            viewDoctors.Filter = UserFilterDoctors;
            viewPatients.Filter = UserFilterPatients;
            viewRooms.Filter = UserFilterRooms;
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private bool UserFilterDoctors(object item)
        {
            if (String.IsNullOrEmpty(DoctorTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(DoctorTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool UserFilterPatients(object item)
        {
            if (String.IsNullOrEmpty(PatientTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(PatientTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool UserFilterRooms(object item)
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

        private int IsPeriodAvailable(Period period) // vraca 0 ako je termin ok, 1 ako je soba zauzeta, 2 ako je doktor zauzet, 3 ako je pacijent zauzet
        {
            if (period.StartTime < DateTime.Now.AddMinutes(15))
            {
                return 4;
            }
            DateTime periodEndtime = period.StartTime.AddMinutes(period.Duration);

            foreach (Period existingPeriod in Model.Resources.periods)
            {
                DateTime existingPeriodEndTime = existingPeriod.StartTime.AddMinutes(existingPeriod.Duration);

                if (period.RoomId == existingPeriod.RoomId)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 1;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 1;
                }

                if (period.DoctorUsername == existingPeriod.DoctorUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 2;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 2;
                }

                if (period.PatientUsername == existingPeriod.PatientUsername)
                {
                    if (period.StartTime >= existingPeriod.StartTime && period.StartTime < existingPeriodEndTime)
                        return 3;

                    if (periodEndtime > existingPeriod.StartTime && periodEndtime < existingPeriodEndTime)
                        return 3;
                }
            }

            return 0;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.Patient == null || this.Doctor == null || this.PeriodTypeIndex == -1 || this.Room == null || this.Date == null)
            {
                MessageBox.Show("Please fill all the fields.", "Fill all the fields");
                return;
            }
            string[] splits = Time.Split(":");
            DateTime date = new DateTime(Date.Year, Date.Month, Date.Day, Int32.Parse(splits[0]), Int32.Parse(splits[1]), 0);
            Period period = new Period(date, Int32.Parse(Duration), (PeriodType)PeriodTypeIndex, Patient.Username, Doctor.Username, Room.Id);
            int available = IsPeriodAvailable(period);

            if (available == 0)
            {
                Model.Resources.OpenPeriods();
                if (Model.Resources.periods == null)
                    Model.Resources.periods = new List<Model.Period>();

                Model.Resources.periods.Add(period);
                Model.Resources.SavePeriods();

                NavigationService.Navigate(new SecretaryPeriodsPage());
            }
            else if (available == 1)
            {
                MessageBox.Show("Selected room is unavailable in selected period.", "Room unavailable");
            }
            else if (available == 2)
            {
                MessageBox.Show("Selected doctor is unavailable in selected period.", "Doctor unavailable");
            }
            else if (available == 3)
            {
                MessageBox.Show("Selected patient is unavailable in selected period.", "Patient unavailable");
            }
            else
            {
                MessageBox.Show("Selected time is not acceptable.", "Time unacceptable");
            }
        }
    }
}
