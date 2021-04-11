using Model;
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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for EditGuestPage.xaml
    /// </summary>
    public partial class EditGuestPage : Page, INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _citizenId;
        private string _healthCardNumber;
        private Patient _selectedPatient;

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
        public string CitizenId
        {
            get => _citizenId;
            set
            {
                _citizenId = value;
                OnPropertyChanged("CitizenId");
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
        

        public void initializeBindingFields()
        {
            PName = SelectedPatient.Name;
            Surname = SelectedPatient.Surname;
            CitizenId = SelectedPatient.CitizenId;
            HealthCardNumber = SelectedPatient.HealthCardNumber;
        }
        public EditGuestPage(Patient selectedPatient)
        {
            InitializeComponent();
            SelectedPatient = selectedPatient;
            initializeBindingFields();
            this.DataContext = this;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = new Patient(PName, Surname, CitizenId, HealthCardNumber);
            patient.MaritalStatus = (MaritalStatus)(-1);
            patient.Gender = (Gender)(-1);
            patient.BloodType = (BloodType)(-1);
            string username = "guest_" + HealthCardNumber;

            ////////////////////////EDITING THE PATIENT INFO////////////////////////////////////

            if (File.Exists(@"..\..\..\Resources\patients.json"))
            {
                Model.Resources.OpenPatients();
                foreach (KeyValuePair<string, Patient> item in Model.Resources.patients)
                {
                    if (item.Key.Equals(username))
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
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }
    }
}
