using Model;
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PatientGeneralService
    {
        private IPatientRepository _patientRepository;
        private ICredentialsRepository _credentialsRepository;
        public PatientGeneralService()
        {
            _patientRepository = new PatientRepository();
            _credentialsRepository = new CredentialsRepository();
        }
        public List<Patient> GetAll()
        {
            return _patientRepository.GetValues();
        }
        public void ProcessPatientDeletion(Patient SelectedPatient)
        {
            if (SelectedPatient == null)
            {
                MessageBox.Show("Select a patient.");
            }
            else
            {
                if (SelectedPatient.IsGuest)
                {
                    _patientRepository.DeleteById("guest_" + SelectedPatient.HealthCardNumber);
                }
                else
                {
                    _patientRepository.DeleteById(SelectedPatient.Username);
                    _credentialsRepository.DeleteById(SelectedPatient.Username);
                }
            }
        }
        public void ProcessPatientUnblock(Patient patientToUnblock)
        {
            patientToUnblock.RecentActions = 0;
            _patientRepository.Update(patientToUnblock);
        }
    }
}
