using Model;
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Data;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class PatientsViewVM : BindableBase
    {
        private ObservableCollection<Patient> _patientsForTable;
        public ObservableCollection<Patient> PatientsForTable { get => _patientsForTable; set => _patientsForTable = value; }
        public PatientGeneralService PatientService { get; set; }
        public Patient SelectedPatient { get; set; }
        private string _patientsSearchText;

        public string PatientsSearchText
        {
            get { return _patientsSearchText; }
            set 
            {
                _patientsSearchText = value;
                OnPropertyChanged("PatientsSearchText");
                if (CollectionViewSource.GetDefaultView(PatientsForTable) != null)
                {
                    CollectionViewSource.GetDefaultView(PatientsForTable).Refresh();
                }
            }
        }


        public PatientsViewVM()
        {
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            PatientService = new PatientGeneralService(patientRepository, credentialsRepository);

            PatientsForTable = new ObservableCollection<Patient>(PatientService.GetAll());
            SelectedPatient = new Patient();
            ICollectionView viewPatients = (ICollectionView)CollectionViewSource.GetDefaultView(PatientsForTable);
            viewPatients.Filter = PatientsFilter;
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
            if(SelectedPatient != null)
            {
                SecretaryWindowVM.CustomYesNoDialog = new CustomYesNoDialog("Are you sure?", "Action cannot be undone.");
                SecretaryWindowVM.CustomYesNoDialog.Owner = SecretaryWindowVM.SecretaryWindow;

                if ((bool)SecretaryWindowVM.CustomYesNoDialog.ShowDialog())
                {
                    PatientService.ProcessPatientDeletion(SelectedPatient);

                    PatientsForTable.Remove(SelectedPatient);
                }
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Hint", "Select a patient first.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
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
        private bool PatientsFilter(object item)
        {
            if (String.IsNullOrEmpty(PatientsSearchText))
                return true;
            else
                return (((item as Patient).Name).IndexOf(PatientsSearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (((item as Patient).Surname).IndexOf(PatientsSearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (((item as Patient).CitizenId).IndexOf(PatientsSearchText, StringComparison.OrdinalIgnoreCase) >= 0) ||
                    (((item as Patient).HealthCardNumber).IndexOf(PatientsSearchText, StringComparison.OrdinalIgnoreCase) >= 0);
        }
    }
}
