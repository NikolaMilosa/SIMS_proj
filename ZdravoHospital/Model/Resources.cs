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
        public Dictionary<string, Doctor> Doctors { get; set; }
        public Dictionary<int, AppointmentRoom> AppoinmentRooms { get; set; }
        public Dictionary<int, OperatingRoom> OperatingRooms { get; set; }
        public List<Appointment> Appointments { get; set; }
        public List<Operation> Operations { get; set; }


        public Resources()
        {
            Accounts = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(File.ReadAllText(@"..\..\..\Resources\accounts.json"));
        }
    }
}