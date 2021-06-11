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
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryPeriodsPage.xaml
    /// </summary>
    public partial class SecretaryPeriodsPage : Page, INotifyPropertyChanged
    {
        public PeriodsService PeriodsService { get; set; }
        public ObservableCollection<Period> Periods { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public Period SelectedPeriod { get; set; }
        private DateTime _selectedDate { get; set; }
        public DateTime SelectedDate
        {
            get => _selectedDate;
            set
            {
                _selectedDate = value;
                OnPropertyChanged("SelectedDate");
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

        public SecretaryPeriodsPage()
        {
            InitializeComponent();
            this.DataContext = this;

            IPeriodRepository periodRepository = RepositoryFactory.CreatePeriodRepository();
            IDoctorRepository doctorRepository = RepositoryFactory.CreateDoctorRepository();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            IRoomRepository roomRepository = RepositoryFactory.CreateRoomRepository();
            PeriodsService = new PeriodsService(doctorRepository, patientRepository, periodRepository, roomRepository);
            
            SelectedDate = DateTime.Today;

            Periods = new ObservableCollection<Period>(PeriodsService.GetPeriods());
            Doctors = new ObservableCollection<Doctor>(PeriodsService.GetDoctors());
            Patients = new ObservableCollection<Patient>(PeriodsService.GetPatients());

            ICollectionView viewDoctors = (ICollectionView)CollectionViewSource.GetDefaultView(Doctors);
            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(Patients);

            viewDoctors.Filter = UserFilterDoctors;
            viewPatients.Filter = UserFilterPatients;

            CollectionView viewPeriods = (CollectionView)CollectionViewSource.GetDefaultView(Periods);
            viewPeriods.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
            viewPeriods.Filter = UserFilterPeriods;
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
        private bool UserFilterPeriods(object item)
        {
            if (DoctorsListBox.SelectedItem == null && PatientsListBox.SelectedItem == null)
                return (item as Period).StartTime.Date.CompareTo(SelectedDate.Date) == 0;
            else if (DoctorsListBox.SelectedItem != null && PatientsListBox.SelectedItem == null)
                return ((item as Period).DoctorUsername.IndexOf(((Doctor)DoctorsListBox.SelectedItem).Username, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (item as Period).StartTime.Date == SelectedDate.Date;
            else if (DoctorsListBox.SelectedItem == null && PatientsListBox.SelectedItem != null)
                return ((item as Period).PatientUsername.IndexOf(((Patient)PatientsListBox.SelectedItem).Username, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (item as Period).StartTime.Date == SelectedDate.Date;
            else
                return ((item as Period).DoctorUsername.IndexOf(((Doctor)DoctorsListBox.SelectedItem).Username, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    ((item as Period).PatientUsername.IndexOf(((Patient)PatientsListBox.SelectedItem).Username, StringComparison.OrdinalIgnoreCase) >= 0) &&
                    (item as Period).StartTime.Date == SelectedDate.Date;
        }

        private void DoctorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(DoctorsListBox.ItemsSource).Refresh();
        }

        private void PatientTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PatientsListBox.ItemsSource).Refresh();
        }

        private void YesterdayButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(-1);
        }

        private void TomorrowButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedDate = SelectedDate.AddDays(1);
        }

        private void NewPeriodButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecretaryNewPeriodPage());
        }

        private void DeletePeriodButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedPeriod != null)
            {
                Period period = (Period)PeriodsListView.SelectedItem;
                

                SecretaryWindowVM.CustomYesNoDialog = new CustomYesNoDialog("Are you sure?", "Action cannot be undone.");
                SecretaryWindowVM.CustomYesNoDialog.Owner = SecretaryWindowVM.SecretaryWindow;

                if((bool)SecretaryWindowVM.CustomYesNoDialog.ShowDialog())
                {
                    Periods.Remove(period);
                    PeriodsService.ProcessPeriodDeletion(period.PeriodId);
                    CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource).Refresh();
                }
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select a period first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
            
        }

        private void DoctorsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource).Refresh();
        }

        private void PatientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource).Refresh();
        }

        private void ResetViewButton_Click(object sender, RoutedEventArgs e)
        {
            PatientsListBox.SelectedItem = null;
            DoctorsListBox.SelectedItem = null;
            SelectedDate = DateTime.Today;
        }

        private void AppointmentDatePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            if(CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource) != null)
            {
                CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource).Refresh();
            }
        }
    }
}
