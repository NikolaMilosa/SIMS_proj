using System;
using System.Collections.Generic;
using System.Windows;

using Model;
using Repository.CredentialsPersistance;
using ZdravoHospital.GUI.DoctorUI;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.GUI.PatientUI;
using ZdravoHospital.GUI.PatientUI.View;
using ZdravoHospital.GUI.Secretary;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            var credentialsRepository = new CredentialsRepository();
            var credentials = credentialsRepository.GetById(username);

            if (credentials == null)
            {
                MessageBox.Show("Username not registered...");
            }
            else
            {
                if (credentials.Password.Equals(password))
                {
                    App.currentUser = username;
                    Window window = null;

                    switch (credentials.Role)
                    {
                        case RoleType.MANAGER:
                            window = new Wizard(username);
                            break;
                        case RoleType.DOCTOR:
                            window = new DoctorWindow();
                            break;
                        case RoleType.SECERATRY:
                            window = new SecretaryWindow(username);
                            break;
                        case RoleType.PATIENT:
                            window = new PatientWindow(username);
                            break;
                    }

                    window.Show();
                    this.Close();
                }
                else
                {
                    MessageBox.Show("Wrong password...");
                }
            }
        }

    }
}
