using System;
using System.Collections.Generic;
using System.IO;
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
using Newtonsoft.Json;
using ZdravoHospital.GUI.ManagerUI;

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
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordTextBox.Password;

            try
            {
                if (Model.Resources.accounts[username].Password.Equals(password))
                {
                    Window window = null;
                    switch (Model.Resources.accounts[username].Role)
                    {
                        case RoleType.MANAGER:
                            window = new ManagerWindow(username);
                            break;
                        case RoleType.DOCTOR:
                            break;
                        case RoleType.SECERATRY:
                            break;
                        case RoleType.PATIENT:
                            break;
                    }
                    window.Show();
                    this.Close();
                    
                    //Emptying the dictionary for memory reasons

                    Model.Resources.accounts.Clear();
                    Model.Resources.accounts = null;
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
