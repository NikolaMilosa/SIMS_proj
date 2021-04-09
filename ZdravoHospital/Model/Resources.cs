using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Model
{
    public static class Resources
    {
        public static Dictionary<string, Credentials> accounts;
        public static Dictionary<string, Employee> employees;
        public static Dictionary<string, Patient> patients;
        public static Dictionary<string, Doctor> doctors;
        public static Dictionary<int, Room> rooms;
        public static List<Period> periods;
        public static List<Notification> notifications;
        public static Dictionary<string,Inventory> inventory;
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

        public static Employee findManager(string username)
        {
            employees = JsonConvert.DeserializeObject<Dictionary<string, Employee>>(File.ReadAllText(@"..\..\..\Resources\employees.json"));
            Employee sol = employees[username];
            employees.Clear();
            employees = null;
            return sol;
        }

        public static void OpenRooms()
        {
            rooms = JsonConvert.DeserializeObject<Dictionary<int, Room>>(File.ReadAllText(@"..\..\..\Resources\rooms.json"));
            if (rooms == null)
                rooms = new Dictionary<int, Room>();
        }

        public static void SerializeRooms()
        {
            File.WriteAllText(@"..\..\..\Resources\rooms.json", JsonConvert.SerializeObject(rooms));
        }

        public static void OpenInventory()
        {
            inventory = JsonConvert.DeserializeObject<Dictionary<string,Inventory>>(File.ReadAllText(@"..\..\..\Resources\inventory.json"));
            if (inventory == null)
                inventory = new Dictionary<string, Inventory>();
        }

        public static void SerializeInventory()
        {
            File.WriteAllText(@"..\..\..\Resources\inventory.json", JsonConvert.SerializeObject(inventory));
        }

        internal static void CloseAllManager()
        {
            //Closer for all lists
            if(rooms != null)
            {
                rooms.Clear();
                rooms = null;
            }
            if(inventory != null)
            {
                inventory.Clear();
                inventory = null;
            }
        }

        public static void SerializeDoctors()
        {
            File.WriteAllText(@"..\..\..\Resources\doctors.json", JsonConvert.SerializeObject(doctors));
        }

        public static void DeserializeDoctors()
        {
            doctors = JsonConvert.DeserializeObject<Dictionary<string, Doctor>>(File.ReadAllText(@"..\..\..\Resources\doctors.json"));

            if (doctors == null)
                doctors = new Dictionary<string, Doctor>();
        }
    }
}