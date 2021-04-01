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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientRegistrationPage.xaml
    /// </summary>
    public partial class PatientRegistrationPage : Page, INotifyPropertyChanged
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
        private string _citizenId;
        private string _country;
        private string _city;
        private int _postalCode;

        private string _healthCardNumber;   // sve osim ovoga u klasi person
        private string _parentsName;
        private MaritalStatus _maritalStatus;
        private Gender _gender;
        private BloodType _bloodType;
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

        public BloodType BloodType
        {
            get { return _bloodType; }
            set 
            { 
                _bloodType = value;
                OnPropertyChanged("BloodType");
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
        public string CitizenId
        {
            get => _citizenId;
            set
            {
                _citizenId = value;
                OnPropertyChanged("CitizenId");
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

        public string HealthCardNumber
        {
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
        public MaritalStatus MaritalStatus
        {
            get => _maritalStatus;
            set
            {
                _maritalStatus = value;
                OnPropertyChanged("MaritalStatus");
            }
        }
        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                OnPropertyChanged("Gender");
            }
        }

        public Credentials Credentials { get => _credentials; set => _credentials = value; }

        public PatientRegistrationPage()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        public bool isUsernameUnique(Dictionary<string, Credentials> credentials, string username)
        {
            foreach (KeyValuePair<string, Credentials> item in credentials)
            {
                if (item.Key.Equals(username))
                {
                    return false;
                }
            }
            return true;
        }


        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient(HealthCardNumber, PName, Surname, Email, DateOfBirth, Telephone, Username, ParentsName, (MaritalStatus)MaritalStatusComboBox.SelectedIndex, (Gender)GenderComboBox.SelectedIndex, CitizenId, (BloodType)BloodTypeComboBox.SelectedIndex);

            patient.Address = new Address(StreetName, StreetNum,
                new Model.City(PostalCode, this.City, new Model.Country(this.Country)));

            if (patient.Username.Equals("") || Password.Equals(""))
            {
                MessageBox.Show("Username and password are required fields.");
            }
            else
            {
                ////////////////////////ADDING A NEW ACCOUNT//////////////////////////////////
                Credentials = new Credentials(Username, Password, RoleType.PATIENT);
                Model.Resources.OpenAccounts();
                if (!isUsernameUnique(Model.Resources.accounts, Username))
                {
                    MessageBox.Show("Username already exists in the system.");
                }
                else
                {
                    Model.Resources.accounts.Add(Username, this.Credentials);
                    Model.Resources.SaveAccounts();

                    ////////////////////////ADDING A NEW PATIENT///////////////////////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        Model.Resources.patients.Add(Username, patient);
                        Model.Resources.SavePatients();
                    }
                    else
                    {
                        Model.Resources.patients = new Dictionary<string, Patient>();
                        Model.Resources.patients.Add(Username, patient);
                        Model.Resources.SavePatients();
                    }

                    MessageBox.Show("Added successfully");
                    NavigationService.Navigate(new SecretaryHomePage());
                }

            }

        }
    }
}
