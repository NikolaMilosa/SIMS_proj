using Model;
using Repository.DoctorPersistance;
using Repository.PatientPersistance;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using Repository.SpecializationPersistance;
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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryUrgentPeriodPage.xaml
    /// </summary>
    public partial class SecretaryUrgentPeriodPage : Page
    {
        public UrgentPeriodsService UrgentService { get; set; }
        public ObservableCollection<Patient> Patients { get; set; }
        public ObservableCollection<Specialization> Specializations { get; set; }
        public UrgentPeriodDTO UrgentPeriodDTO { get; set; }

        
        public SecretaryUrgentPeriodPage()
        {
            InitializeComponent();
            this.DataContext = this;

            IPeriodRepository periodRepository = RepositoryFactory.CreatePeriodRepository();
            IDoctorRepository doctorRepository = RepositoryFactory.CreateDoctorRepository();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            IRoomRepository roomRepository = RepositoryFactory.CreateRoomRepository();
            ISpecializationRepository specializationRepository = RepositoryFactory.CreateSpecializationRepository();
            UrgentService = new UrgentPeriodsService(periodRepository, specializationRepository, patientRepository, doctorRepository, roomRepository);
            
            UrgentPeriodDTO = new UrgentPeriodDTO();

            Patients = new ObservableCollection<Patient>(UrgentService.GetPatients());
            Specializations = new ObservableCollection<Specialization>(UrgentService.GetSpecializations());

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
            if(UrgentPeriodDTO.Patient == null || UrgentPeriodDTO.SelectedSpecialization == null)
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Invalid request", "Please select entities from the lists.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
            else
            {
                PeriodsViewHolderDTO viewHolder = UrgentService.ProcessUrgentPeriodCreation(UrgentPeriodDTO);
                if (viewHolder.Status == UrgentPeriodStatus.NO_DOCTORS_AVAILABLE)
                {
                    SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Sorry", "No doctors available.");
                    SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                    SecretaryWindowVM.CustomMessageBox.Show();
                }

                else if (viewHolder.Status == UrgentPeriodStatus.PERIODS_TO_MOVE)
                    NavigationService.Navigate(new PeriodsToMovePage(viewHolder.Periods));
                else
                    NavigationService.Navigate(new UrgentPeriodSummaryPage(viewHolder.BestPeriod));
            }
            
        }

        
    }
}
