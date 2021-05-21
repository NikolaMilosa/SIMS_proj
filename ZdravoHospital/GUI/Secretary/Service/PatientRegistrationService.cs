using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using ZdravoHospital.GUI.Secretary.DTO;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class PatientRegistrationService
    {
        private Repository.CredentialsPersistance.CredentialsRepository _credentialsRepository;
        private Repository.PatientPersistance.PatientRepository _patientsRepository;
        public PatientRegistrationService()
        {
            _credentialsRepository = new Repository.CredentialsPersistance.CredentialsRepository();
            _patientsRepository = new Repository.PatientPersistance.PatientRepository();
        }

        public void processPatientRegistration(PatientDTO patientDTO)
        {
            bool success = tryCreateAccount(patientDTO);

            if (success)
            {
                registerPatient(patientDTO);
            }

        }

        private bool tryCreateAccount(PatientDTO patientDTO)
        {
            Credentials accountCandidate = new Credentials(patientDTO.Username, patientDTO.Password, RoleType.PATIENT);
            bool isUnique = _credentialsRepository.CreateIfUnique(accountCandidate);
            return isUnique;
        }

        private void registerPatient(PatientDTO patientDTO)
        {
            Patient patient = createPatientFromDTO(patientDTO);
            _patientsRepository.Create(patient);
            MessageBox.Show("Added successfully");
        }

        private AddressDTO createPatientsAddress(PatientDTO patientDTO)
        {
            CountryDTO countryDTO = new CountryDTO(patientDTO.Country);
            CityDTO cityDTO = new CityDTO(patientDTO.PostalCode, patientDTO.City, countryDTO);
            return new AddressDTO(patientDTO.StreetName, patientDTO.StreetNum, cityDTO);
        }

        private Patient createPatientFromDTO(PatientDTO patientDTO)
        {
            AddressDTO addressDTO = createPatientsAddress(patientDTO);
            Patient patient = new Patient(patientDTO.HealthCardNumber, patientDTO.PName, patientDTO.Surname, patientDTO.Email, patientDTO.DateOfBirth, patientDTO.PhoneNumber, patientDTO.Username, patientDTO.ParentsName, patientDTO.MaritalStatus, patientDTO.Gender, patientDTO.CitizenId, patientDTO.BloodType);
            patient.Address = new Address(addressDTO.StreetName, addressDTO.Number,
                new Model.City(addressDTO.CityDTO.PostalCode, addressDTO.CityDTO.Name, new Model.Country(addressDTO.CityDTO.CountryDTO.Name)));
            return patient;
        }
    }
}
