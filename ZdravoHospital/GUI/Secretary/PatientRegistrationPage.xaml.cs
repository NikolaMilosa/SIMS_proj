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
using ZdravoHospital.GUI.Secretary.DTO;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientRegistrationPage.xaml
    /// </summary>
    public partial class PatientRegistrationPage : Page, INotifyPropertyChanged
    {
        private PatientDTO _patientDTO;
        public PatientDTO PatientDTO
        {
            get => _patientDTO;
            set
            {
                _patientDTO = value;
                OnPropertyChanged("PatientDTO");
            }
        }
        public AddressDTO AddressDTO { get; set; }

        private Credentials _credentials;
        public Credentials Credentials { get => _credentials; set => _credentials = value; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }



        public PatientRegistrationPage()
        {
            InitializeComponent();
            this.DataContext = this;
            PatientDTO = new PatientDTO();
        }

        private void initAddress()
        {
            CountryDTO countryDTO = new CountryDTO(PatientDTO.Country);
            CityDTO cityDTO = new CityDTO(PatientDTO.PostalCode, PatientDTO.City, countryDTO);
            AddressDTO = new AddressDTO(PatientDTO.StreetName, PatientDTO.StreetNum, cityDTO);
        }

        private Patient createPatient(PatientDTO dto)
        {
            initAddress();
            Patient patient = new Patient(dto.HealthCardNumber, dto.PName, dto.Surname, dto.Email, dto.DateOfBirth, dto.PhoneNumber, dto.Username, dto.ParentsName, dto.MaritalStatus, dto.Gender, dto.CitizenId, dto.BloodType);
            patient.Address = new Address(AddressDTO.StreetName, AddressDTO.Number,
                new Model.City(AddressDTO.CityDTO.PostalCode, AddressDTO.CityDTO.Name, new Model.Country(AddressDTO.CityDTO.CountryDTO.Name)));
            return patient;
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
            if (PatientDTO.Username.Equals("") || PatientDTO.Password.Equals(""))
            {
                MessageBox.Show("Username and password are required fields.");
            }
            else
            {
                ////////////////////////ADDING A NEW ACCOUNT//////////////////////////////////
                Credentials = new Credentials(PatientDTO.Username, PatientDTO.Password, RoleType.PATIENT);
                Model.Resources.OpenAccounts();
                if (!isUsernameUnique(Model.Resources.accounts, PatientDTO.Username))
                {
                    MessageBox.Show("Username already exists in the system.");
                }
                else
                {
                    Model.Resources.accounts.Add(PatientDTO.Username, this.Credentials);
                    Model.Resources.SaveAccounts();

                    ////////////////////////ADDING A NEW PATIENT///////////////////////////////////
                    if (File.Exists(@"..\..\..\Resources\patients.json"))
                    {
                        Model.Resources.OpenPatients();
                        Patient patient = createPatient(PatientDTO);
                        Model.Resources.patients.Add(PatientDTO.Username, patient);
                        Model.Resources.SavePatients();
                    }
                    else
                    {
                        Patient patient = createPatient(PatientDTO);
                        Model.Resources.patients = new Dictionary<string, Patient>();
                        Model.Resources.patients.Add(PatientDTO.Username, patient);
                        Model.Resources.SavePatients();
                    }

                    MessageBox.Show("Added successfully");
                    //NavigationService.Navigate(new PatientsView());
                }

            }

        }

        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
