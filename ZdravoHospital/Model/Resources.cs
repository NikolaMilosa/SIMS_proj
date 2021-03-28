using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Model
{
    public class Resources
    {
        public static Dictionary<string, Credentials> Accounts { get; set; }              //accounts.json
        public Dictionary<string, Secretary> Secretaries { get; set; }
        public static Dictionary<string, Patient> Patients { get; set; }
        public static Dictionary<string, Doctor> Doctors { get; set; }
        public static Dictionary<string, Specialist> Specialists { get; set; }
        public static Dictionary<int, AppointmentRoom> AppointmentRooms { get; set; }      //appointmentRooms.json
        public static Dictionary<int, OperatingRoom> OperatingRooms { get; set; }          //operatingRooms.json
        public static Dictionary<int, Room> StorageAndBedRooms { get; set; }               //storageAndBedRooms.json
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


            //Doctors = new Dictionary<string, Doctor>();
            //Doctor d = new Doctor("Marko", "Pantelic", "pantela@zdravo.com", DateTime.Now, "063/789789", "pantela", "Jovo", MaritalStatus.MARRIED, Gender.MALE);
            //Doctors[d.Username] = d;
            //Accounts[d.Username] = new Credentials(d.Username, "pantela", RoleType.DOCTOR);

            //Patients = new Dictionary<string, Patient>();
            //Patient p = new Patient("12123434", "Dejan", "Bodiroga", "boga@zdravo.com", DateTime.Now, "063/789789", "bogi", "Sale", MaritalStatus.MARRIED, Gender.MALE, "12345678901");
            //Patients[p.Username] = p;
            //Accounts[p.Username] = new Credentials(p.Username, "bogi", RoleType.PATIENT);

            //Specialists = new Dictionary<string, Specialist>();
            //Specialist s = new Specialist("Nikola", "Zigic", "zigara@zdravo.com", DateTime.Now, "063/456456", "zigara", "Pera", MaritalStatus.MARRIED, Gender.MALE, "Cardio surgent");
            //Specialists[s.Username] = s;
            //Accounts[s.Username] = new Credentials(s.Username, "zigara", RoleType.SPECIALIST);
            //Serialize();
        }

        public static void Serialize()
        {
            File.WriteAllText(@"..\..\..\Resources\accounts.json", JsonConvert.SerializeObject(Accounts, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\patients.json", JsonConvert.SerializeObject(Patients, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\doctors.json", JsonConvert.SerializeObject(Doctors, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\specialists.json", JsonConvert.SerializeObject(Specialists, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\appointmentRooms.json", JsonConvert.SerializeObject(AppointmentRooms, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\operatingRooms.json", JsonConvert.SerializeObject(OperatingRooms, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));
            File.WriteAllText(@"..\..\..\Resources\storageAndBedRooms.json", JsonConvert.SerializeObject(StorageAndBedRooms, Formatting.None,
                        new JsonSerializerSettings()
                        {
                            ReferenceLoopHandling = ReferenceLoopHandling.Ignore
                        }));

        }
    }
}