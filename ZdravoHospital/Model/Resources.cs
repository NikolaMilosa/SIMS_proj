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
        public static Dictionary<string,Inventory> inventory;
        public static List<Medicine> medicines;
        public static List<Ingredient> ingredients;
        public static List<Notification> notifications;
        public static List<PersonNotification> personNotifications;
        public static List<Specialization> specializations;
        public static List<RoomInventory> roomInventory;


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

            if (patients == null)
                patients = new Dictionary<string, Patient>();
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
            File.WriteAllText(@"..\..\..\Resources\rooms.json", JsonConvert.SerializeObject(rooms, Formatting.Indented));
        }

        public static void OpenInventory()
        {
            inventory = JsonConvert.DeserializeObject<Dictionary<string,Inventory>>(File.ReadAllText(@"..\..\..\Resources\inventory.json"));
            if (inventory == null)
                inventory = new Dictionary<string, Inventory>();
        }

        public static void SerializeInventory()
        {
            File.WriteAllText(@"..\..\..\Resources\inventory.json", JsonConvert.SerializeObject(inventory, Formatting.Indented));
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

        public static void SaveNotifications()
        {
            string json = JsonConvert.SerializeObject(notifications);
            File.WriteAllText(@"..\..\..\Resources\notifications.json", json);
        }

        public static void OpenNotifications()
        {
            if(File.Exists(@"..\..\..\Resources\notifications.json"))
                notifications = JsonConvert.DeserializeObject<List<Notification>>(File.ReadAllText(@"..\..\..\Resources\notifications.json"));

            if (notifications == null)
                notifications = new List<Notification>();
        }

        public static void OpenPeriods() 
        {
            if (File.Exists(@"..\..\..\Resources\periods.json"))
                periods = JsonConvert.DeserializeObject<List<Period>>(File.ReadAllText(@"..\..\..\Resources\periods.json"));
            
           if(periods==null)
                periods = new List<Period>();

        }

        public static void SavePeriods() 
        {
            string json = JsonConvert.SerializeObject(periods);
            File.WriteAllText(@"..\..\..\Resources\periods.json", json);
        }

        public static void OpenMedicines()
        {
            if (File.Exists(@"..\..\..\Resources\medicines.json"))
                medicines = JsonConvert.DeserializeObject<List<Medicine>>(File.ReadAllText(@"..\..\..\Resources\medicines.json"));

            if (medicines == null)
                medicines = new List<Medicine>();

        }

        public static void SaveMedicines()
        {
            string json = JsonConvert.SerializeObject(medicines);
            File.WriteAllText(@"..\..\..\Resources\medicines.json", json);
        }

        public static void OpenIngredients()
        {
            if (File.Exists(@"..\..\..\Resources\ingredients.json"))
                ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(File.ReadAllText(@"..\..\..\Resources\ingredients.json"));

            if (ingredients == null)
                ingredients = new List<Ingredient>();

        }

        public static void SaveIngredients()
        {
            string json = JsonConvert.SerializeObject(ingredients);
            File.WriteAllText(@"..\..\..\Resources\ingredients.json", json);
        }


        public static void OpenSpecializations()
        {
            if (File.Exists(@"..\..\..\Resources\specializations.json"))
                specializations = JsonConvert.DeserializeObject<List<Specialization>>(File.ReadAllText(@"..\..\..\Resources\specializations.json"));

            if (specializations == null)
                specializations = new List<Specialization>();
        }

        public static void SaveSpecializations()
        {
            string json = JsonConvert.SerializeObject(specializations);
            File.WriteAllText(@"..\..\..\Resources\specializations.json", json);
        }
        public static void SavePersonNotifications()
        {
            string json = JsonConvert.SerializeObject(personNotifications);
            File.WriteAllText(@"..\..\..\Resources\personNotifications.json", json);
        }

        public static void OpenPersonNotifications()
        {
            if (File.Exists(@"..\..\..\Resources\personNotifications.json"))
                personNotifications = JsonConvert.DeserializeObject<List<PersonNotification>>(File.ReadAllText(@"..\..\..\Resources\personNotifications.json"));

            if (personNotifications == null)
                personNotifications = new List<PersonNotification>();
        }
        public static void OpenRoomInventory()
        {
            roomInventory = JsonConvert.DeserializeObject<List<RoomInventory>>(File.ReadAllText(@"..\..\..\Resources\roomInventory.json"));
            if (roomInventory == null)
                roomInventory = new List<RoomInventory>();
        }

        public static void SerializeRoomInventory()
        {
            File.WriteAllText(@"..\..\..\Resources\roomInventory.json", JsonConvert.SerializeObject(roomInventory, Formatting.Indented));

        }
    }
}