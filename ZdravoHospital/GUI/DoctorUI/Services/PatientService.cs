using Model;
using Repository.PatientPersistance;
using System.Collections.Generic;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class PatientService
    {
        private PatientRepository _patientRepository;

        public PatientService()
        {
            _patientRepository = new PatientRepository();
        }

        public  List<Patient> GetPatients()
        {
            return _patientRepository.GetValues();
        }

        public Patient GetPatient(string patientUsername)
        {
            return _patientRepository.GetById(patientUsername);
        }
    }
}
