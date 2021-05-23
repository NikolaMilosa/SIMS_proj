using Model;
using Repository.DoctorPersistance;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.Service
{
    public class WorkTimeService
    {
        private IDoctorRepository _doctorRepository;
        public WorkTimeService()
        {
            _doctorRepository = new DoctorRepository();
        }
        public List<Doctor> GetAllDoctors()
        {
            return _doctorRepository.GetValues();
        }
    }
}
