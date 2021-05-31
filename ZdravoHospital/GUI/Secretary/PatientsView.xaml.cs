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
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Page
    {
        /*private ObservableCollection<Patient> _patientsForTable;
        public ObservableCollection<Patient> PatientsForTable { get => _patientsForTable; set => _patientsForTable = value; }
        public PatientGeneralService PatientService { get; set; }
        public Patient SelectedPatient { get; set; }*/

        public PatientsView()
        {
            InitializeComponent();
            DataContext = new PatientsViewVM();
            //PatientService = new PatientGeneralService();
            //PatientsForTable = new ObservableCollection<Patient>(PatientService.GetAll());
        }

        /*private void DeletePatientButton_Click(object sender, RoutedEventArgs e)
        {
            PatientService.ProcessPatientDeletion(SelectedPatient);
            //delete from view
            if (SelectedPatient != null)
                PatientsForTable.Remove(SelectedPatient);
        }

        private void DetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var chosenPatient = (sender as Button).DataContext as Patient;
            NavigationService.Navigate(new PatientDetailsPage(chosenPatient));
        }*/

        private void PatientsSearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        /*private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            var patientToUnblock = (sender as Button).DataContext as Patient;
            PatientService.ProcessPatientUnblock(patientToUnblock);
            CollectionViewSource.GetDefaultView(PatientsListView.ItemsSource).Refresh();
        }*/

        /*private void SelectCurrentItem(object sender, KeyboardFocusChangedEventArgs e)
        {
            ListViewItem item = (ListViewItem)sender;
            item.IsSelected = true;
            MessageBox.Show("SSADSDA");
        }*/

    }
}
