using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
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
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Page
    {
        private ObservableCollection<Patient> _patientsForTable;
        public ObservableCollection<Patient> PatientsForTable { get => _patientsForTable; set => _patientsForTable = value; }
        public ObservableCollection<Patient> dictionaryToList(Dictionary<String, Patient> Patients)
        {
            ObservableCollection<Patient> ret = new ObservableCollection<Patient>();
            foreach (KeyValuePair<string, Patient> pair in Patients)
            {
                ret.Add(pair.Value);
            }
            return ret;
        }
        public PatientsView()
        {
            InitializeComponent();
            this.DataContext = this;
            Model.Resources.OpenPatients();
            PatientsForTable = dictionaryToList(Model.Resources.patients);
        }

        private void DeletePatientButton_Click(object sender, RoutedEventArgs e)
        {
            //this.PatientsListView.ItemsSource = PatientsForTable;

            var selectedPatient = ((Patient)PatientsListView.SelectedItem);
            if (selectedPatient == null)
            {
                
            }
            else
            {
                if (selectedPatient.IsGuest)
                {
                    PatientsForTable.Remove(selectedPatient);
                    Model.Resources.patients.Remove("guest_" + selectedPatient.HealthCardNumber);
                    Model.Resources.SavePatients();
                    return;
                }
                else
                {
                    ///////     DELETE FROM TABLE AND LIST OF PATIENTS      //////
                    PatientsForTable.Remove(selectedPatient);
                    Model.Resources.patients.Remove(selectedPatient.Username);
                    Model.Resources.SavePatients();

                    ///////     DELETE FROM ACCOUNTS    ////////
                    Model.Resources.OpenAccounts();
                    Model.Resources.accounts.Remove(selectedPatient.Username);
                    Model.Resources.SaveAccounts();
                }

            }
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (sender as Button).DataContext as Patient;
            NavigationService.Navigate(new PatientDetailsPage(selectedPatient));
        }

        private void PatientsSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedPatient = (sender as Button).DataContext as Patient;
            selectedPatient.RecentActions = 0;
            CollectionViewSource.GetDefaultView(PatientsListView.ItemsSource).Refresh();
            foreach(KeyValuePair<string, Patient> item in Model.Resources.patients)
            {
                if (item.Key.Equals(selectedPatient.Username))
                {
                    item.Value.RecentActions = 0;
                }
            }
            Model.Resources.SavePatients();
        }
    }
}
