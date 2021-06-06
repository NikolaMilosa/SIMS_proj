using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ZdravoHospital.GUI.Secretary.Commands;
using ZdravoHospital.GUI.Secretary.DTO;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class PatientRegistrationVM : BindableBase
    {
        private PatientDTO _patientDTO;
        public PatientDTO PatientDTO
        {
            get => _patientDTO;
            set
            {
                _patientDTO = value;
                OnPropertyChanged("PatientDTO");
            }
        }


        public PatientRegistrationService PatientService { get; set; }

        public PatientRegistrationVM()
        {
            PatientDTO = new PatientDTO();
            PatientService = new PatientRegistrationService();
            FinishCommand = new RelayCommand(finishExecute);
        }

        public ICommand FinishCommand { get; set; }

        private void finishExecute(object parameter)
        {
            PatientService.processPatientRegistration(PatientDTO);
            SecretaryWindowVM.NavigationService.Navigate(new PatientsView());
        }
    }
}
