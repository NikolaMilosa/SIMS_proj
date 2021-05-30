using Model;
using System.Collections.Generic;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class PatientController
    {
        private PatientService _patientService;

        public PatientController()
        {
            _patientService = new PatientService();
        }

        public List<Patient> GetPatients()
        {
            return _patientService.GetPatients();
        }

        public Patient GetPatient(string patientUsername)
        {
            return _patientService.GetPatient(patientUsername);
        }
    }
}
