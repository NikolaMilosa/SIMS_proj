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

        public string GetName()
        {
            return Fullname.Split(" ")[0];
        }

        public string GetSurname()
        { 
            return Fullname.Split(" ")[1];
        }
    }
}
