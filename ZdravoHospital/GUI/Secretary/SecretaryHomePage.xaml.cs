using System;
using System.Collections.Generic;
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
    /// Interaction logic for SecretaryHomePage.xaml
    /// </summary>
    public partial class SecretaryHomePage : Page
    {
        public SecretaryHomePage()
        {
            InitializeComponent();
        }

        private void OpenMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //CloseMenuButton.Visibility = Visibility.Visible;
            //OpenMenuButton.Visibility = Visibility.Collapsed;
        }

        private void CloseMenuButton_Click(object sender, RoutedEventArgs e)
        {
            //CloseMenuButton.Visibility = Visibility.Collapsed;
            //OpenMenuButton.Visibility = Visibility.Visible;
        }

        
    }
}
