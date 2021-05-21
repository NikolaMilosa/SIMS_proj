using Repository.CredentialsPersistance;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
    /// Interaction logic for PatientDetailsPage.xaml
    /// </summary>
    
    public partial class PatientDetailsPage : Page
    {
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
        public Model.Patient Patient { get; set; }
        private string _password;
        public string Password
        {
            get => _password;
            set
            {
                _password = value;
                OnPropertyChanged("Password");
            }
        }
        public PatientDetailsPage(Model.Patient patient)
        {
            InitializeComponent();
            Patient = patient;
            this.DataContext = this;
            if (!Patient.IsGuest)
            {
                this.initPassword();
                this.hidePassword();
            }
            
        }
        private void initPassword()
        {
            CredentialsRepository credentialsRepository = new CredentialsRepository();
            foreach (var item in credentialsRepository.GetValues())
            {
                if (item.Username.Equals(Patient.Username))
                {
                    Password = item.Password;
                    break;
                }
            }
        }

        private void hidePassword()
        {
            StringBuilder passwordBuilder = new StringBuilder();
            for(int i = 0; i < Password.Length; ++i) {
                passwordBuilder.Append("*");
            }
            PasswordTextBlock.Text = passwordBuilder.ToString();
            ShowPasswordIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Show;
        }

        private void showPassword()
        {
            PasswordTextBlock.Text = Password;
            ShowPasswordIcon.Kind = MaterialDesignThemes.Wpf.PackIconKind.Hide;
        }

        private void ShowPasswordButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Patient.IsGuest)
            {
                if (ShowPasswordIcon.Kind == MaterialDesignThemes.Wpf.PackIconKind.Show)
                {
                    showPassword();
                }
                else
                {
                    hidePassword();
                }
            }
            
        }

        private void EditPatientButton_Click(object sender, RoutedEventArgs e)
        {
            if (!Patient.IsGuest)
            {
                NavigationService.Navigate(new EditPatientPage(Patient));
            }
            else
            {
                NavigationService.Navigate(new EditGuestPage(Patient));
            }
        }

        private void EditAllergiesButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AllergiesPage(this.Patient));
        }
    }
}
