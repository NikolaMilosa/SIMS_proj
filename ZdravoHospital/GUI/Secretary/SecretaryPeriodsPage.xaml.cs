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
    /// Interaction logic for SecretaryPeriodsPage.xaml
    /// </summary>
    public partial class SecretaryPeriodsPage : Page, INotifyPropertyChanged
    {
        public ObservableCollection<Period> Periods { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
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
            SelectedDate = DateTime.Today;
            Model.Resources.OpenPeriods();
            Periods = new ObservableCollection<Period>(Model.Resources.periods);

            if (Model.Resources.doctors == null)
                Model.Resources.DeserializeDoctors();
            if (Model.Resources.patients == null)
                Model.Resources.OpenPatients();
            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            Patients = new ObservableCollection<Patient>(Model.Resources.patients.Values);

            ICollectionView viewDoctors = (ICollectionView)CollectionViewSource.GetDefaultView(Doctors);
            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(Patients);

            viewDoctors.Filter = UserFilterDoctors;
            viewPatients.Filter = UserFilterPatients;

            CollectionView viewPeriods = (CollectionView)CollectionViewSource.GetDefaultView(Periods);
            viewPeriods.SortDescriptions.Add(new SortDescription("StartTime", ListSortDirection.Ascending));
            viewPeriods.Filter = UserFilterPeriods;
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
            if(PeriodsListView.SelectedItem != null)
            {
                Period period = (Period)PeriodsListView.SelectedItem;
                Periods.Remove(period);
                Model.Resources.periods.Remove(period);

                Model.Resources.SavePeriods();
                CollectionViewSource.GetDefaultView(PeriodsListView.ItemsSource).Refresh();
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
