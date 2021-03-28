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
using System.Windows.Shapes;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for EditGuest.xaml
    /// </summary>
    public partial class EditGuest : Window, INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _personID;
        private string _healthCardNumber;
        private Patient _selectedPatient;
        private PatientsView _parentPage;

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
        public Patient SelectedPatient
        {
            get { return _selectedPatient; }
            set
            {
                _selectedPatient = value;
                OnPropertyChanged("SelectedPatient");
            }
        }
        public PatientsView ParentPage { get => _parentPage; set => _parentPage = value; }

        public void initializeBindingFields()
        {
            PName = SelectedPatient.PName;
            Surname = SelectedPatient.Surname;
            PersonID = SelectedPatient.PersonID;
            HealthCardNumber = SelectedPatient.HealthCardNumber;
        }
        public EditGuest(Patient selectedPatient, PatientsView parentPage)
        {
            InitializeComponent();
            SelectedPatient = selectedPatient;
            initializeBindingFields();
            ParentPage = parentPage;
            this.DataContext = this;
        }

        private void btnFinish_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient(PName, Surname, PersonID, HealthCardNumber);
            patient.MaritalStatus = (MaritalStatus)(-1);
            patient.Gender = (Gender)(-1);
            string username = "guest_" + HealthCardNumber;

            ////////////////////////EDITING THE PATIENT INFO////////////////////////////////////
            Dictionary<string, Patient> patientsForSerialization = new Dictionary<string, Patient>();

            if (File.Exists(@"..\..\..\Resources\patients.json"))
            {
                patientsForSerialization = JsonConvert.DeserializeObject<Dictionary<string, Patient>>(File.ReadAllText(@"..\..\..\Resources\patients.json"));
                foreach (KeyValuePair<string, Patient> item in patientsForSerialization)
                {
                    if (item.Key.Equals(username))
                    {
                        patientsForSerialization[item.Key] = patient;
                        break;
                    }
                }
                string patientsJson = JsonConvert.SerializeObject(patientsForSerialization);
                File.WriteAllText(@"..\..\..\Resources\patients.json", patientsJson);
                ParentPage.patientsDataGrid.ItemsSource = ParentPage.dictionaryToList(patientsForSerialization);
                ParentPage.PatientsForTable = ParentPage.dictionaryToList(patientsForSerialization);
                MessageBox.Show("Successfuly changed.");
                this.Close();
            }
        }
    }
}
