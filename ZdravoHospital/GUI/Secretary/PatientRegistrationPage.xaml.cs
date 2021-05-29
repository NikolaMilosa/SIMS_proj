using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Text;
using System.Threading;
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
using ZdravoHospital.GUI.Secretary.Service;

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
        public Patient PatientDEMO { get; set; }

        public PatientRegistrationService PatientService { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public PatientRegistrationPage(bool isDemoMode = false)
        {
            InitializeComponent();
            this.DataContext = this;
            PatientDTO = new PatientDTO();
            PatientService = new PatientRegistrationService();
            PatientDEMO = PatientService.GetById("aca1999");
            if (isDemoMode)
                ExecuteDemo();
            
        }

        public void ExecuteDemo()
        {
            /*Thread firstNameThread = new Thread(() => textBoxDemo(FirstNameTextBox, PatientDEMO.Name));
            firstNameThread.Start();
            Thread parentsNameThread = new Thread(() => textBoxDemo(ParentsNameTextBox, PatientDEMO.ParentsName));
            parentsNameThread.Start();
            Thread surnameThread = new Thread(() => textBoxDemo(LastNameTextBox, PatientDEMO.Surname));
            surnameThread.Start();*/
            Thread thread = new Thread(StartThreads);
            thread.Start();
        }

        public void StartThreads()
        {
            textBoxDemo(FirstNameTextBox, PatientDEMO.Name);
            textBoxDemo(ParentsNameTextBox, PatientDEMO.ParentsName);
            textBoxDemo(LastNameTextBox, PatientDEMO.Surname);
            textBoxDemo(CitizenIdTextBox, PatientDEMO.CitizenId);
            datepickerDemo();
            textBoxDemo(HealthCardNumberTextBox, PatientDEMO.HealthCardNumber);
            comboboxDemo(BloodTypeComboBox);
            comboboxDemo(GenderComboBox);
            scroll(200, 450);
            comboboxDemo(MaritalStatusComboBox);
            textBoxDemo(EmailTextBox, PatientDEMO.Email);
            textBoxDemo(PhoneNumberTextBox, PatientDEMO.PhoneNumber);
            textBoxDemo(UsernameTextBox, PatientDEMO.Username);
            textBoxDemo(PasswordTextBox, "password123");
            scroll(450, 700);
            textBoxDemo(CountryTextBox, PatientDEMO.Address.City.Country.Name);
            textBoxDemo(CityTextBox, PatientDEMO.Address.City.Name);
            textBoxDemo(PostalCodeTextBox, PatientDEMO.Address.City.PostalCode.ToString());
            textBoxDemo(StreetNameTextBox, PatientDEMO.Address.StreetName);
            textBoxDemo(StreetNumberTextBox, PatientDEMO.Address.Number);
            buttonDemo();
        }

        private void textBoxDemo(TextBox textBox, string value)
        {
            for (int i = 1; i <= value.Length; i++)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    textBox.Text = value.Substring(0, i);
                }));
                Thread.Sleep(150);
            }
        }

        private void datepickerDemo()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                DateOfBirthPicker.IsDropDownOpen = true;
                DateOfBirthPicker.Text = PatientDEMO.DateOfBirth.Date.ToString();
            }));
            Thread.Sleep(300);
            this.Dispatcher.Invoke((Action)(() =>
            {
                DateOfBirthPicker.IsDropDownOpen = false;
            }));

        }
        private void comboboxDemo(ComboBox comboBox)
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                comboBox.IsDropDownOpen = true;
            }));
            Thread.Sleep(450);
            this.Dispatcher.Invoke((Action)(() =>
            {
                comboBox.SelectedIndex = 1;
                comboBox.IsDropDownOpen = false;
            }));
        }

        private void scroll(int from, int to)
        {
            for(int i = from; i < to; i++)
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    Scroller.ScrollToVerticalOffset(i);
                }));
                Thread.Sleep(2);
            }
        }

        private void buttonDemo()
        {
            this.Dispatcher.Invoke((Action)(() =>
            {
                FinishButton.Background = Brushes.GreenYellow;
            }));
            Thread.Sleep(300);
            this.Dispatcher.Invoke((Action)(() =>
            {
                BrushConverter bc = new BrushConverter();
                FinishButton.Background = (Brush)bc.ConvertFrom("#4267B2");
            }));
        }


        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            PatientService.processPatientRegistration(PatientDTO);
            NavigationService.Navigate(new PatientsView());
        }

    }
}
