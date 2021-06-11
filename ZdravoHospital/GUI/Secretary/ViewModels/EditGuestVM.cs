using Model;
using Repository.PatientPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class EditGuestVM
    {
        public Patient SelectedPatient { get; set; }
        public GuestDTO Guest { get; set; }
        public GuestService GuestService { get; set; }
        public EditGuestVM(Patient selectedPatient)
        {
            SelectedPatient = selectedPatient;

            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            GuestService = new GuestService(patientRepository);
            
            Guest = new GuestDTO(SelectedPatient.Name, SelectedPatient.Surname, SelectedPatient.CitizenId, SelectedPatient.HealthCardNumber);
            EditGuestCommand = new RelayCommand(editGuestExecute);
        }

        public ICommand EditGuestCommand { get; set; }

        private void editGuestExecute(object parameter)
        {
            GuestService.ProcessGuestUpdate(Guest);
            SecretaryWindowVM.NavigationService.Navigate(new PatientsView());
        }
    }
}
