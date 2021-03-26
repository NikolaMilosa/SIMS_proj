using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Model;
using Newtonsoft.Json;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for PatientRegistrationPage.xaml
    /// </summary>
    public partial class PatientRegistrationPage : Page
    {
        private string _name;
        private string _surname;
        private string _username;
        private string _password;
        private string _telephone;
        private string _email;
        private string _streetName;
        private string _streetNum;
        private DateTime _dateOfBirth;
        private string _personID;
        private string _country;
        private string _city;
        private int _postalCode;

        private string _healthCardNumber;   // sve osim ovoga u klasi person
        private string _parentsName;
        private MaritalStatus _maritalStatus;
        private Gender gender;
        private Credentials _credentials;

        public static ObservableCollection<Patient> Patients { get; set; }

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
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public string Telephone
        {
            get => _telephone;
            set
            {
                _telephone = value;
                OnPropertyChanged("Telephone");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                OnPropertyChanged("Email");
            }
        }
        public string StreetName
        {
            get => _streetName;
            set
            {
                _streetName = value;
                OnPropertyChanged("StreetName");
            }
        }
        public string StreetNum
        {
            get => _streetNum;
            set
            {
                _streetNum = value;
                OnPropertyChanged("StreetNum");
            }
        }
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                OnPropertyChanged("DateOfBirth");
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
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                OnPropertyChanged("Country");
            }
        }
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                OnPropertyChanged("City");
            }
        }
        public int PostalCode
        {
            get => _postalCode;
            set
            {
                _postalCode = value;
                OnPropertyChanged("PostalCode");
            }
        }

        public string HealthCardNumber { 
            get => _healthCardNumber;
            set
            {
                _healthCardNumber = value;
                OnPropertyChanged("HealthCardNumber");
            }
        }
        public string ParentsName
        {
            get => _parentsName; 
            set
            {
                _parentsName = value;
                OnPropertyChanged("ParentsName");
            }
        }
        public MaritalStatus PMaritalStatus
        {
            get => _maritalStatus; 
            set
            {
                _maritalStatus = value;
                OnPropertyChanged("PMaritalStatus");
            }
        }
        public Gender PGender
        {
            get => gender; 
            set
            {
                gender = value;
                OnPropertyChanged("PGender");
            }
        }

        public Credentials Credentials { get => _credentials; set => _credentials = value; }

        public PatientRegistrationPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }




        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient(HealthCardNumber, PName, Surname, Email, DateOfBirth, Telephone, Username, ParentsName, (MaritalStatus)cbMaritalStatus.SelectedIndex, (Gender)cbGender.SelectedIndex);
            //MessageBox.Show(patient.Username + "  " + patient.MaritalStatus);
            patient.Address = new Adress(StreetName, StreetNum, 
                new Model.City(PostalCode, this.City, new Model.Country(this.Country)));
            //MessageBox.Show(patient.Address.City.Country.name);
            //MessageBox.Show(patient.DateOfBirth.ToString());

            ////////////////////////ADDING A NEW ACCOUNT//////////////////////////////////
            Credentials = new Credentials(Username, Password, RoleType.PATIENT);
            Dictionary<string, Credentials> accounts = new Dictionary<string, Credentials>();
            accounts = JsonConvert.DeserializeObject<Dictionary<string, Credentials>>(File.ReadAllText(@"..\..\..\Resources\accounts.json"));
            accounts.Add(Username, this.Credentials);
            string accountsJson = JsonConvert.SerializeObject(accounts);
            File.WriteAllText(@"..\..\..\Resources\accounts.json", accountsJson);

            ////////////////////////ADDING A NEW PATIENT////////////////////////////////////
            Dictionary<string, Patient> patientsForSerialization = new Dictionary<string, Patient>();

            if (File.Exists(@"..\..\..\Resources\patients.json"))
            {
                patientsForSerialization = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));
                patientsForSerialization.Add(Username, patient);
                string patientsJson = JsonConvert.SerializeObject(patientsForSerialization);
                File.WriteAllText(@"..\..\..\Resources\accounts.json", patientsJson);
            }
            else
            {
                patientsForSerialization.Add(Username, patient);
                string patientsJson = JsonConvert.SerializeObject(patientsForSerialization);
                File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);
            }

            MessageBox.Show("Added successfully");
            NavigationService.Navigate(new SecretaryHomePage());
        }
    }
}
