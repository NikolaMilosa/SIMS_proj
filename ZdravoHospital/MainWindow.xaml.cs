﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Resources res;
        public static RoleType ActiveRole { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            res = new Resources();
        }

        private void loginButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (res.Accounts[usernameTextBox.Text].Password == passwordTextBox.Password)
                {
                    ActiveRole = res.Accounts[usernameTextBox.Text].Role;

                    switch (ActiveRole)
                    {
                        case RoleType.MANAGER:
                            var window = new ManagerWindow(res);
                            window.Show();
                            this.Close();
                            break;
                        case RoleType.SECRETARY:
                            var secretaryWindow = new SecretaryWindow();
                            secretaryWindow.Show();
                            this.Visibility = Visibility.Hidden;
                            break;
                        case RoleType.PATIENT:
                            var patientWindow = new PatientWindow();
                            patientWindow.Show();
                            // this.Visibility = Visibility.Hidden;
                            Close();
                        break;
                        case RoleType.DOCTOR:
                            var doctorWindow = new DoctorWindow();
                            doctorWindow.Show();
                            this.Visibility = Visibility.Hidden;
                            break;
                        case RoleType.SPECIALIST:
                            var specialistWindow = new DoctorWindow();
                            specialistWindow.Show();
                            this.Visibility = Visibility.Hidden;
                            break;
                    }
                }
                else
                    MessageBox.Show("Neuspesno logovanje...");
            } catch (Exception exp)
            {
                MessageBox.Show("Nije registrovan nalog...");
            }
        }
    }
}
