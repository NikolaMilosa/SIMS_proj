using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class PatientsViewVM
    {
        private ObservableCollection<Patient> _patientsForTable;
        public ObservableCollection<Patient> PatientsForTable { get => _patientsForTable; set => _patientsForTable = value; }
        public PatientGeneralService PatientService { get; set; }
        public Patient SelectedPatient { get; set; }
        public PatientsViewVM()
        {
            PatientService = new PatientGeneralService();
            PatientsForTable = new ObservableCollection<Patient>(PatientService.GetAll());
            SelectedPatient = new Patient();
            initializeCommands();
        }

        public ICommand DeletePatientCommand { get; set; }
        public ICommand PatientDetailsCommand { get; set; }
        public ICommand UnblockPatientCommand { get; set; }

        private void initializeCommands()
        {
            DeletePatientCommand = new RelayCommand(deletePatientExecute);
            PatientDetailsCommand = new RelayCommand(detailsPatientExecute);
            UnblockPatientCommand = new RelayCommand(unblockPatientExecute);
        }

        private void deletePatientExecute(object sender)
        {
            SecretaryWindowVM.CustomYesNoDialog = new CustomYesNoDialog("Are you sure?", "Action cannot be undone.");
            SecretaryWindowVM.CustomYesNoDialog.Owner = SecretaryWindowVM.SecretaryWindow;

            if ((bool)SecretaryWindowVM.CustomYesNoDialog.ShowDialog())
            {
                PatientService.ProcessPatientDeletion(SelectedPatient);
                //delete from view
                if (SelectedPatient != null)
                    PatientsForTable.Remove(SelectedPatient);
            }
            
        }

        private void detailsPatientExecute(object sender)
        {
            var selected = sender as Patient;
            SecretaryWindowVM.NavigationService.Navigate(new PatientDetailsPage(selected));
        }
        private void unblockPatientExecute(object sender)
        {
            var selected = sender as Patient;
            PatientService.ProcessPatientUnblock(selected);
            CollectionViewSource.GetDefaultView(PatientsForTable).Refresh();
        }
    }
}
