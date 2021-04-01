using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Model
{
    public static class Resources
    {
        public static Dictionary<string, Credentials> accounts;
        public static Dictionary<string, Secretary> secretaries;
        public static Dictionary<string, Patient> patients;
        public static Dictionary<string, Doctor> doctors;
        public static Dictionary<string, Manager> managers;
        public static List<Period> periods;
        public static List<Notification> notifications;
        public static void OpenAccounts()
        {
            accounts = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(File.ReadAllText(@"..\..\..\Resources\accounts.json"));
        }

        public static void SaveAccounts()
        {
            string json = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(@"..\..\..\Resources\accounts.json", json);
        }

        public static void OpenPatients()
        {
            patients = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));
        }

        public static void SavePatients()
        {
            string json = JsonConvert.SerializeObject(patients);
            File.WriteAllText(@"..\..\..\Resources\patients.json", json);
        }
        

    }
}