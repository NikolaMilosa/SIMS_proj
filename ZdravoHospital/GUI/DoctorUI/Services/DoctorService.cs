using Model;
using Model.Repository;
using System.Collections.Generic;
using System.Linq;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class DoctorService
    {
        private DoctorRepository _doctorRepository;

        public DoctorService()
        {
            _doctorRepository = new DoctorRepository();
        }

        public List<Doctor> GetDoctors()
        {
            return _doctorRepository.GetValues();
        }

        public List<Doctor> GetSpecialists()
        {
            return _doctorRepository.GetValues().Where(d => !d.SpecialistType.Equals("Doctor")).ToList();
        }
    }
}
