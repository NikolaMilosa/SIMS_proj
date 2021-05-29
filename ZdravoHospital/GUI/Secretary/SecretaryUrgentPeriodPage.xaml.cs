using Model;
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
using ZdravoHospital.GUI.Secretary.Service;

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
            UrgentService = new UrgentPeriodsService();
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
            PeriodsViewHolderDTO viewHolder = UrgentService.ProcessUrgentPeriodCreation(UrgentPeriodDTO);
            if(viewHolder.Status == UrgentPeriodStatus.NO_DOCTORS_AVAILABLE)
                MessageBox.Show("Sorry, no doctors available.");
            else if(viewHolder.Status == UrgentPeriodStatus.PERIODS_TO_MOVE)
                NavigationService.Navigate(new PeriodsToMovePage(viewHolder.Periods));
            else
                NavigationService.Navigate(new UrgentPeriodSummaryPage(viewHolder.BestPeriod));
        }

        
    }
}
