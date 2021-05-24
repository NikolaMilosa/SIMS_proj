using Model;
using System;
using System.Collections.Generic;
using System.Text;
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
    }
}
