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
    /// Interaction logic for GuestAccountPage.xaml
    /// </summary>
    public partial class GuestAccountPage : Page, INotifyPropertyChanged
    {
        private string _name;
        private string _surname;
        private string _citizenId;
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

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            Patient guestPatient = new Patient(PName, Surname, CitizenId, HealthCardNumber);
            guestPatient.Gender = (Gender)(-1);
            guestPatient.MaritalStatus = (MaritalStatus)(-1);
            guestPatient.BloodType = (BloodType)(-1);

            string guestID = "guest_" + HealthCardNumber;


            if (File.Exists(@"..\..\..\Resources\patients.json"))
            {
                Model.Resources.OpenPatients();
                if (HealthCardNumber.Equals("") || !isHealthCardUnique(Model.Resources.patients, HealthCardNumber))
                {
                    MessageBox.Show("Health card number must be unique.");
                }
                else
                {
                    Model.Resources.patients.Add(guestID, guestPatient);
                    Model.Resources.SavePatients();
                    MessageBox.Show("Added successfully");
                    NavigationService.Navigate(new SecretaryHomePage());
                }

            }
            else
            {
                Model.Resources.patients = new Dictionary<string, Patient>();
                Model.Resources.patients.Add(guestID, guestPatient);
                Model.Resources.SavePatients();
                MessageBox.Show("Added successfully");
                NavigationService.Navigate(new SecretaryHomePage());
            }


        }
    }
}
