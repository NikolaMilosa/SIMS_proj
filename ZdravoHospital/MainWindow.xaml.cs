using System;
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
using ZdravoHospital.GUI.DoctorUI;
using ZdravoHospital.GUI.ManagerUI;
using ZdravoHospital.GUI.Secretary;

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
            Model.Resources.OpenAccounts();

            //Doctor d1 = new Doctor("Marko", "Pantelic", "pantela", "Doctor");
            //Doctor d2 = new Doctor("Nikola", "Zigic", "zigara", "Cardio surgent");
            //Model.Resources.doctors = new Dictionary<string, Doctor>();
            //Model.Resources.doctors[d1.Username] = d1;
            //Model.Resources.doctors[d2.Username] = d2;
            //Model.Resources.accounts[d1.Username] = new Credentials(d1.Username, "pantela", RoleType.DOCTOR);
            //Model.Resources.accounts[d2.Username] = new Credentials(d2.Username, "zigara", RoleType.DOCTOR);
            //Model.Resources.SerializeDoctors();
            //Model.Resources.SaveAccounts();
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            String username = UsernameTextBox.Text;
            String password = PasswordTextBox.Password;

            try
            {
                if (Model.Resources.accounts[username].Password.Equals(password))
                {
                    App.currentUser = username;
                    Window window = null;

                    switch (Model.Resources.accounts[username].Role)
                    {
                        case RoleType.MANAGER:
                            window = new ManagerWindow(username);
                            break;
                        case RoleType.DOCTOR:
                            window = new DoctorWindow();
                            break;
                        case RoleType.SECERATRY:
                            window = new SecretaryWindow(username);
                            break;
                        case RoleType.PATIENT:
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
            catch
            {
                MessageBox.Show("Username not registered...");
            }
        }
    }
}
