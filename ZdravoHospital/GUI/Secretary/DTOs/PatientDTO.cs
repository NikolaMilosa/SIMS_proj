using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace ZdravoHospital.GUI.Secretary.DTO
{
    public class PatientDTO : BindableBase
    {
        private string _name;
        private string _surname;
        private string _username;
        private string _password;
        private string _phoneNumber;
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


        public AddressDTO Address { get; set; }
        public BloodType BloodType
        {
            get { return _bloodType; }
            set
            {
                _bloodType = value;
                //OnPropertyChanged("BloodType");
            }
        }
        public string PName
        {
            get { return _name; }
            set
            {
                _name = value;
                //OnPropertyChanged("PName");
            }
        }
        public string Surname
        {
            get { return _surname; }
            set
            {
                _surname = value;
                //OnPropertyChanged("Surname");
            }
        }
        public string Username
        {
            get => _username;
            set
            {
                _username = value;
                //OnPropertyChanged("Username");
            }
        }
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                //OnPropertyChanged("Password");
            }
        }
        public string PhoneNumber
        {
            get => _phoneNumber;
            set
            {
                _phoneNumber = value;
                //OnPropertyChanged("Telephone");
            }
        }
        public string Email
        {
            get => _email;
            set
            {
                _email = value;
                //OnPropertyChanged("Email");
            }
        }
        public string StreetName
        {
            get => _streetName;
            set
            {
                _streetName = value;
                //OnPropertyChanged("StreetName");
            }
        }
        public string StreetNum
        {
            get => _streetNum;
            set
            {
                _streetNum = value;
                //OnPropertyChanged("StreetNum");
            }
        }
        public DateTime DateOfBirth
        {
            get => _dateOfBirth;
            set
            {
                _dateOfBirth = value;
                //OnPropertyChanged("DateOfBirth");
            }
        }
        public string CitizenId
        {
            get => _citizenId;
            set
            {
                _citizenId = value;
                //OnPropertyChanged("CitizenId");
            }
        }
        public string Country
        {
            get => _country;
            set
            {
                _country = value;
                //OnPropertyChanged("Country");
            }
        }
        public string City
        {
            get => _city;
            set
            {
                _city = value;
                //OnPropertyChanged("City");
            }
        }
        public int PostalCode
        {
            get => _postalCode;
            set
            {
                _postalCode = value;
                //OnPropertyChanged("PostalCode");
            }
        }

        public string HealthCardNumber
        {
            get => _healthCardNumber;
            set
            {
                _healthCardNumber = value;
                //OnPropertyChanged("HealthCardNumber");
            }
        }
        public string ParentsName
        {
            get => _parentsName;
            set
            {
                _parentsName = value;
                //OnPropertyChanged("ParentsName");
            }
        }
        public MaritalStatus MaritalStatus
        {
            get => _maritalStatus;
            set
            {
                _maritalStatus = value;
                //OnPropertyChanged("MaritalStatus");
            }
        }
        public Gender Gender
        {
            get => _gender;
            set
            {
                _gender = value;
                //OnPropertyChanged("Gender");
            }
        }
        public Credentials Credentials { get => _credentials; set => _credentials = value; }

        public PatientDTO()
        {

        }

        public PatientDTO(BloodType bloodType, string pName, string surname, string username, string password, string phoneNumber, string email, string streetName, string streetNum, DateTime dateOfBirth, string citizenId, string country, string city, int postalCode, string healthCardNumber, string parentsName, MaritalStatus maritalStatus, Gender gender, Credentials credentials)
        {
            BloodType = bloodType;
            PName = pName;
            Surname = surname;
            Username = username;
            Password = password;
            PhoneNumber = phoneNumber;
            Email = email;
            StreetName = streetName;
            StreetNum = streetNum;
            DateOfBirth = dateOfBirth;
            CitizenId = citizenId;
            Country = country;
            City = city;
            PostalCode = postalCode;
            HealthCardNumber = healthCardNumber;
            ParentsName = parentsName;
            MaritalStatus = maritalStatus;
            Gender = gender;
            Credentials = credentials;
        }
    }
}
