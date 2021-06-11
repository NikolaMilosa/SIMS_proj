using Model;
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
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
using ZdravoHospital.GUI.Secretary.DTO;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;

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

        public static ObservableCollection<Patient> Patients { get; set; }
        public EditPatientService PatientService;

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
                CredentialsRepository credentialsRepository = new CredentialsRepository();
                foreach (var item in credentialsRepository.GetValues())
                {
                    if (item.Username.Equals(Username))
                    {
                        Password = item.Password;
                        OldPassword = item.Password;
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
            MaritalStatus = (MaritalStatus)SelectedPatient.MaritalStatus;
            Gender = (Gender)SelectedPatient.Gender;
            GenderComboBox.SelectedIndex = (int)Gender;
            MaritalStatusComboBox.SelectedIndex = (int)MaritalStatus;
            BloodTypeComboBox.SelectedIndex = (int)BloodType;
            
        }

        public Credentials Credentials { get => _credentials; set => _credentials = value; }
        public string OldPassword { get => _oldPassword; set => _oldPassword = value; }
        public EditPatientPage(Patient selectedPatient)
        {
            InitializeComponent();
            SelectedPatient = selectedPatient;
            initializeBindingFields();

            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();

            PatientService = new EditPatientService(patientRepository, credentialsRepository);
            this.DataContext = this;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            PatientDTO patientDTO = new PatientDTO((BloodType)BloodTypeComboBox.SelectedIndex, PName, Surname, Username, Password, Telephone, Email, StreetName, StreetNum, DateOfBirth, CitizenId, Country, City, PostalCode, HealthCardNumber, ParentsName, (MaritalStatus)MaritalStatusComboBox.SelectedIndex, (Gender)GenderComboBox.SelectedIndex, Credentials);
            PatientService.ProcessPatientEdit(patientDTO);
            NavigationService.Navigate(new PatientsView());
        }

    }
}
