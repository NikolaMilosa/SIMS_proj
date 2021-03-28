using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Model
{
    public class Resources
    {
        public Dictionary<string, Credentials> Accounts { get; set; }              //accounts.json
        public Dictionary<string, Secretary> Secretaries { get; set; }
        public Dictionary<string, Patient> Patients { get; set; }
        public static Dictionary<string, Doctor> Doctors { get; set; }
        public static Dictionary<string, Specialist> Specialists { get; set; }
        public Dictionary<int, AppointmentRoom> AppointmentRooms { get; set; }      //appointmentRooms.json
        public Dictionary<int, OperatingRoom> OperatingRooms { get; set; }          //operatingRooms.json
        public Dictionary<int, Room> StorageAndBedRooms { get; set; }               //storageAndBedRooms.json
        public List<Appointment> Appointments { get; set; }
        public List<Operation> Operations { get; set; }


        public Resources()
        {
            Accounts = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(File.ReadAllText(@"..\..\..\Resources\accounts.json"));
            
            Patients = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));

            Doctors = JsonConvert.DeserializeObject<Dictionary<string, Doctor>>(File.ReadAllText(@"..\..\..\Resources\doctors.json"));

            Specialists = JsonConvert.DeserializeObject<Dictionary<string, Specialist>>(File.ReadAllText(@"..\..\..\Resources\specialists.json"));

            AppointmentRooms = JsonConvert.DeserializeObject<Dictionary<int, AppointmentRoom>>(File.ReadAllText(@"..\..\..\Resources\appointmentRooms.json"));

            OperatingRooms = JsonConvert.DeserializeObject<Dictionary<int, OperatingRoom>>(File.ReadAllText(@"..\..\..\Resources\operatingRooms.json"));

            StorageAndBedRooms = JsonConvert.DeserializeObject<Dictionary<int, Room>>(File.ReadAllText(@"..\..\..\Resources\storageAndBedRooms.json"));

            //Specialists = new Dictionary<string, Specialist>();
            //Specialist s = new Specialist("Nikola", "Zigic", "zigara@zdravo.com", DateTime.Now, "063/456456", "zigara", "Pera", MaritalStatus.MARRIED, Gender.MALE, "Cardio surgent");
            //Specialists[s.Username] = s;
            //serialize();
        }

        public void serialize()
        {
            File.WriteAllText(@"..\..\..\Resources\patients.json", JsonConvert.SerializeObject(Patients));
            File.WriteAllText(@"..\..\..\Resources\doctors.json", JsonConvert.SerializeObject(Doctors));
            File.WriteAllText(@"..\..\..\Resources\specialists.json", JsonConvert.SerializeObject(Specialists));
            File.WriteAllText(@"..\..\..\Resources\appointmentRooms.json", JsonConvert.SerializeObject(AppointmentRooms));
            File.WriteAllText(@"..\..\..\Resources\operatingRooms.json", JsonConvert.SerializeObject(OperatingRooms));
            File.WriteAllText(@"..\..\..\Resources\storageAndBedRooms.json", JsonConvert.SerializeObject(StorageAndBedRooms));
            
        }
    }
}