using Model;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for EditPatientPage.xaml
    /// </summary>
    public partial class EditPatientPage : Page, INotifyPropertyChanged
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

        private Patient _selectedPatient;
        private string _oldPassword;
        private PatientsView _parentPage;

        public static ObservableCollection<Patient> Patients { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged("SelectedPatient");
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
        public BloodType BloodType
        {
            get => _bloodType;
            set
            {
                _bloodType = value;
                OnPropertyChanged("BloodType");
            }
        }

        public void initializeBindingFields()
        {
            PName = SelectedPatient.Name;
            Surname = SelectedPatient.Surname;
            Username = SelectedPatient.Username;
            Password = "";
            try
            {
                Model.Resources.OpenAccounts();
                foreach (KeyValuePair<string, Credentials> item in Model.Resources.accounts)
                {
                    if (item.Key.Equals(Username))
                    {
                        Password = item.Value.Password;
                        OldPassword = item.Value.Password;
                        break;
                    }
                }
            }
            catch (Exception e)
            {

            }
            Telephone = SelectedPatient.PhoneNumber;
            Email = SelectedPatient.Email;
            if (SelectedPatient.Address != null)
            {
                StreetName = SelectedPatient.Address.StreetName;
                StreetNum = SelectedPatient.Address.Number;
                if (SelectedPatient.Address.City != null)
                {
                    City = SelectedPatient.Address.City.Name;
                    PostalCode = SelectedPatient.Address.City.PostalCode;
                    if (SelectedPatient.Address.City.Country != null)
                    {
                        Country = SelectedPatient.Address.City.Country.Name;
                    }
                }
            }
            if (SelectedPatient.DateOfBirth != null)
                DateOfBirth = SelectedPatient.DateOfBirth;

            CitizenId = SelectedPatient.CitizenId;
            HealthCardNumber = SelectedPatient.HealthCardNumber;
            ParentsName = SelectedPatient.ParentsName;
            MaritalStatus = SelectedPatient.MaritalStatus;
            Gender = SelectedPatient.Gender;
            GenderComboBox.SelectedIndex = (int)SelectedPatient.Gender;
            MaritalStatusComboBox.SelectedIndex = (int)SelectedPatient.MaritalStatus;
            BloodTypeComboBox.SelectedIndex = (int)SelectedPatient.BloodType;
        }

        public Credentials Credentials { get => _credentials; set => _credentials = value; }
        public string OldPassword { get => _oldPassword; set => _oldPassword = value; }
        public EditPatientPage(Patient selectedPatient)
        {
            InitializeComponent();
            SelectedPatient = selectedPatient;
            initializeBindingFields();
            this.DataContext = this;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient(HealthCardNumber, PName, Surname, Email, DateOfBirth, Telephone, Username, ParentsName, (MaritalStatus)MaritalStatusComboBox.SelectedIndex, (Gender)GenderComboBox.SelectedIndex, CitizenId, (BloodType)BloodTypeComboBox.SelectedIndex);
            
            patient.Address = new Address(StreetName, StreetNum,
                new Model.City(PostalCode, this.City, new Model.Country(this.Country)));

            if (Password.Equals(""))
            {
                MessageBox.Show("Password is a required field.");
            }
            else
            {
                ////////////////////////EDITING THE ACCOUNT CREDENTIALS//////////////////////////////////
                if (!OldPassword.Equals(Password))
                {
                    Credentials = new Credentials(Username, Password, RoleType.PATIENT);
                    Model.Resources.OpenAccounts();
                    foreach (KeyValuePair<string, Credentials> item in Model.Resources.accounts)
                    {
                        if (item.Key.Equals(Username))
                        {
                            item.Value.Password = Password;
                            break;
                        }
                    }
                    Model.Resources.SaveAccounts();
                }


                ////////////////////////EDITING THE PATIENT INFO////////////////////////////////////
                //Dictionary<string, Patient> patientsForSerialization = new Dictionary<string, Patient>();

                if (File.Exists(@"..\..\..\Resources\patients.json"))
                {
                    Model.Resources.OpenPatients();
                    foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                    {
                        if (item.Key.Equals(Username))
                        {
                            Model.Resources.patients[item.Key] = patient;
                            break;
                        }
                    }
                    Model.Resources.SavePatients();
                    MessageBox.Show("Successfuly changed.");
                    NavigationService.Navigate(new PatientsView());
                }
            }
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
}
}
