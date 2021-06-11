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
using Repository.CredentialsPersistance;
using Repository.PatientPersistance;
using ZdravoHospital.GUI.Secretary.DTO;
using ZdravoHospital.GUI.Secretary.Factory;
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientRegistrationPage.xaml
    /// </summary>
    public partial class PatientRegistrationPage : Page
    {
        
        public PatientRegistrationPage(bool isDemoMode = false)
        {
            InitializeComponent();
            this.DataContext = new PatientRegistrationVM();
            IPatientRepository patientRepository = RepositoryFactory.CreatePatientRepository();
            ICredentialsRepository credentialsRepository = RepositoryFactory.CreateCredentialsRepository();
            PatientDEMO = new PatientRegistrationService(credentialsRepository, patientRepository).GetById("aca1999");
            if (isDemoMode)
                ExecuteDemo(); 
        }

        #region DEMO
        public Patient PatientDEMO { get; set; }
        public Thread DemoThread { get; set; }
        public void ExecuteDemo()
        {
            DemoThread = new Thread(CallDemoMethods);
            DemoThread.Start();
        }

        public void CallDemoMethods()
        {
            while (true)
            {
                toggleStopVisibility();
                disableComponents();
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
                scrollToTop();
                clearComponents();
                executeCountdown();
            }
        }

        private void toggleStopVisibility()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    StopDemoButton.Visibility = Visibility.Visible;
                    SecondsLeftTextBlock.Visibility = Visibility.Visible;
                }));
            }catch(Exception ex) { }
            
        }

        private void executeCountdown()
        {
            try
            {
                for (int i = 5; i >= 0; --i)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        SecondsLeftTextBlock.Text = i.ToString();
                    }));
                    Thread.Sleep(1000);
                }
            }catch(Exception ex) { }
            

        }

        private void disableComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    FirstNameTextBox.IsReadOnly = true;
                    ParentsNameTextBox.IsReadOnly = true;
                    LastNameTextBox.IsReadOnly = true;
                    CitizenIdTextBox.IsReadOnly = true;
                    DateOfBirthPicker.IsEnabled = false;
                    HealthCardNumberTextBox.IsReadOnly = true;
                    BloodTypeComboBox.IsEnabled = false;
                    GenderComboBox.IsEnabled = false;
                    MaritalStatusComboBox.IsEnabled = false;
                    EmailTextBox.IsReadOnly = true;
                    PhoneNumberTextBox.IsReadOnly = true;
                    UsernameTextBox.IsReadOnly = true;
                    PasswordTextBox.IsReadOnly = true;
                    CountryTextBox.IsReadOnly = true;
                    CityTextBox.IsReadOnly = true;
                    PostalCodeTextBox.IsReadOnly = true;
                    StreetNameTextBox.IsReadOnly = true;
                    StreetNumberTextBox.IsReadOnly = true;
                    FinishButton.IsEnabled = false;
                }));
            }catch(Exception ex) { }
            
        }

        private void clearComponents()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    FirstNameTextBox.Text = "";
                    ParentsNameTextBox.Text = "";
                    LastNameTextBox.Text = "";
                    CitizenIdTextBox.Text = "";
                    DateOfBirthPicker.SelectedDate = null;
                    HealthCardNumberTextBox.Text = "";
                    BloodTypeComboBox.SelectedIndex = -1;
                    GenderComboBox.SelectedIndex = -1;
                    MaritalStatusComboBox.SelectedIndex = -1;
                    EmailTextBox.Text = "";
                    PhoneNumberTextBox.Text = "";
                    UsernameTextBox.Text = "";
                    PasswordTextBox.Text = "";
                    CountryTextBox.Text = "";
                    CityTextBox.Text = "";
                    PostalCodeTextBox.Text = "";
                    StreetNameTextBox.Text = "";
                    StreetNumberTextBox.Text = "";
                }));
            }
            catch (Exception ex) { }

        }

        private void textBoxDemo(TextBox textBox, string value)
        {
            try
            {
                for (int i = 1; i <= value.Length; i++)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        textBox.Text = value.Substring(0, i);
                    }));
                    Thread.Sleep(150);
                }
            }catch(Exception ex) { }
            
        }

        private void datepickerDemo()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    DateOfBirthPicker.IsEnabled = true;
                    DateOfBirthPicker.IsDropDownOpen = true;
                    DateOfBirthPicker.Text = PatientDEMO.DateOfBirth.Date.ToString();
                }));
                Thread.Sleep(300);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    DateOfBirthPicker.IsDropDownOpen = false;
                    DateOfBirthPicker.IsEnabled = false;
                }));
            }catch(Exception ex)
            {

            }
           

        }
        private void comboboxDemo(ComboBox comboBox)
        {
            try
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
            }catch(Exception ex) { }
            
        }

        private void scroll(int from, int to)
        {
            try
            {
                for (int i = from; i < to; i++)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Scroller.ScrollToVerticalOffset(i);
                    }));
                    Thread.Sleep(2);
                }
            }catch(Exception ex) { }
            
        }

        private void buttonDemo()
        {
            try
            {
                this.Dispatcher.Invoke((Action)(() =>
                {
                    FinishButton.IsEnabled = true;
                    FinishButton.Background = Brushes.GreenYellow;
                }));
                Thread.Sleep(300);
                this.Dispatcher.Invoke((Action)(() =>
                {
                    BrushConverter bc = new BrushConverter();
                    FinishButton.Background = (Brush)bc.ConvertFrom("#4267B2");
                    FinishButton.IsEnabled = false;
                }));
            }
            catch (Exception ex) { }
            
        }

        private void scrollToTop()
        {
            try
            {
                for (int i = 700; i >= 0; i--)
                {
                    this.Dispatcher.Invoke((Action)(() =>
                    {
                        Scroller.ScrollToVerticalOffset(i);
                    }));
                    Thread.Sleep(2);
                }
            }catch(Exception ex) { }
            
        }

        private void StopDemoButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DemoThread.Abort();
            }catch(Exception ex) { }
           
            NavigationService.Navigate(new DemoPage());
        }

        #endregion
    }
}
