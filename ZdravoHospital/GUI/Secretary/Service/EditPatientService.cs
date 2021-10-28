using Model;
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
using ZdravoHospital.GUI.Secretary.DTO;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class EditPatientService
    {
        IPatientRepository _patientsRepository;
        ICredentialsRepository _credentialsRepository;
        public EditPatientService(IPatientRepository patientRepository, ICredentialsRepository credentialsRepository)
        {
            _patientsRepository = patientRepository;
            _credentialsRepository = credentialsRepository;
        }

        public void ProcessPatientEdit(PatientDTO patientDTO)
        {
            UpdateAccount(patientDTO);
            updatePatient(patientDTO);
        }

        private void UpdateAccount(PatientDTO patientDTO)
        {
            Credentials editedAccount = new Credentials(patientDTO.Username, patientDTO.Password, RoleType.PATIENT);
            _credentialsRepository.Update(editedAccount);
        }

        private void updatePatient(PatientDTO patientDTO)
        {
            Patient patient = createPatientFromDTO(patientDTO);
            _patientsRepository.Update(patient);
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
