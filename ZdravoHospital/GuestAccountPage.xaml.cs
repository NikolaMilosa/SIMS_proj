using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for GuestAccountPage.xaml
    /// </summary>
    public partial class GuestAccountPage : Page, INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _personID;
        private string _healthCardNumber;

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public string PName
        {
            get { return _name; }
            set
            {
                _name = value;
                OnPropertyChanged("PName");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                OnPropertyChanged("Surname");
            }
        }

        public string PersonID
        {
            get => _personID;
            set
            {
                _personID = value;
                OnPropertyChanged("PersonID");
            }
        }

        public string HealthCardNumber
        {
            get => _healthCardNumber;
            set
            {
                _healthCardNumber = value;
                OnPropertyChanged("HealthCardNumber");
            }
        }
        public GuestAccountPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public bool isHealthCardUnique(Dictionary<string, Patient> patients, string healthCardNum)
        {
            foreach (KeyValuePair<string, Patient> item in patients)
            {
                if (item.Value.HealthCardNumber.Equals(healthCardNum))
                {
                    return false;
                }
            }
            return true;
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Patient guestPatient = new Patient(PName, Surname, PersonID, HealthCardNumber);
            Dictionary<string, Patient> patientsForSerialization = new Dictionary<string, Patient>();
            
            string guestID = "guest_" + HealthCardNumber;
            

            if (File.Exists(@"..\..\..\Resources\patients.json"))
            {
                patientsForSerialization = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));
                if (HealthCardNumber.Equals("") || !isHealthCardUnique(patientsForSerialization, HealthCardNumber))
                {
                    MessageBox.Show("Health card number must be unique.");
                }
                else
                {
                    patientsForSerialization.Add(guestID, guestPatient);
                    string patientsJson = JsonConvert.SerializeObject(patientsForSerialization);
                    File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);
                    MessageBox.Show("Added successfully");
                    NavigationService.Navigate(new SecretaryHomePage());
                }
                
            }
            else
            {
                patientsForSerialization.Add(guestID, guestPatient);
                string patientsJson = JsonConvert.SerializeObject(patientsForSerialization);
                File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);
                MessageBox.Show("Added successfully");
                NavigationService.Navigate(new SecretaryHomePage());
            }

            
        }
    }
}
