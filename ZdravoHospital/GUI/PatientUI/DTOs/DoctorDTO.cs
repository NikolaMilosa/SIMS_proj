using Model;

namespace ZdravoHospital.GUI.PatientUI.DTOs
{
    public class DoctorDTO
    {
        public string Fullname { get; set; }
        public string Username { get; set; }

        public DoctorDTO(Doctor doctor) 
        {
            Fullname = doctor.Name + " " + doctor.Surname;
            Username = doctor.Username;
        }
    }
}
