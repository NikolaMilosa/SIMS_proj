using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Model;
using Repository.DoctorPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class DoctorFunctions
    {
        public DoctorRepository DoctorRepository { get; private set; }

        public DoctorFunctions()
        {
            DoctorRepository = new DoctorRepository();
        }

        public List<Doctor> GetGeneralDoctors()
        {
            var doctors = DoctorRepository.GetValues();
            return doctors.Where(doctor => doctor.SpecialistType.SpecializationName.Equals("Doctor")).ToList();
        }

        public List<Doctor> GetAllDoctors()
        {
            return DoctorRepository.GetValues();
        }

        public Doctor GetDoctor(string username)
        {
            var doctors = DoctorRepository.GetValues();
            return doctors.FirstOrDefault(doctor => doctor.Username.Equals(username));
        }

        public string GetDoctorUsername(string name, string surname)
        {
            return (from doctor in DoctorRepository.GetValues() where doctor.Name.Equals(name) && doctor.Surname.Equals(surname) select doctor.Username).FirstOrDefault();
        }

    }
}
