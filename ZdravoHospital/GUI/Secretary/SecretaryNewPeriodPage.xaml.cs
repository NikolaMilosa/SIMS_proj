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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryNewPeriodPage.xaml
    /// </summary>
    public partial class SecretaryNewPeriodPage : Page
    {
        public PeriodsService PeriodsService { get; set; }
        public PeriodDTO PeriodDTO { get; set; }
        public ObservableCollection<Doctor> Doctors { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Room> Rooms { get; set; }

        


        public SecretaryNewPeriodPage()
        {
            InitializeComponent();
            this.DataContext = this;
            PeriodsService = new PeriodsService();
            PeriodDTO = new PeriodDTO();
            initializeListsForBinding();
            setSearchFilters();
        }


        private void initializeListsForBinding()
        {
            Doctors = new ObservableCollection<Doctor>(PeriodsService.GetDoctors());
            Patients = new ObservableCollection<Patient>(PeriodsService.GetPatients());
            Rooms = new ObservableCollection<Room>(PeriodsService.GetRooms());
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


        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = PeriodsService.ProcessPeriodCreation(PeriodDTO);
            if(success)
                NavigationService.Navigate(new SecretaryPeriodsPage());
        }
    }
}
