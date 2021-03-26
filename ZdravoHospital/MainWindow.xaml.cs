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

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Resources res;
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
                    switch (res.Accounts[usernameTextBox.Text].Role)
                    {
                        case RoleType.MANAGER:
                            var window = new ManagerWindow();
                            window.Show();
                            this.Visibility = Visibility.Hidden;
                            break;
                        case RoleType.SECRETARY:
                            var secretaryWindow = new SecretaryWindow();
                            secretaryWindow.Show();
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
