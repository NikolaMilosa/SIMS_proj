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

        public static Manager findManager(string username)
        {
            managers = JsonConvert.DeserializeObject<Dictionary<string, Manager>>(File.ReadAllText(@"..\..\..\Resources\managers.json"));
            Manager sol = managers[username];
            managers.Clear();
            managers = null;
            return sol;
        }

    }
}