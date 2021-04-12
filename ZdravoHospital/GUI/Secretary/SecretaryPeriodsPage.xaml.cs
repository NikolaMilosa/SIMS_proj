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
    /// Interaction logic for SecretaryPeriodsPage.xaml
    /// </summary>
    public partial class SecretaryPeriodsPage : Page
    {
        public SecretaryPeriodsPage()
        {
            InitializeComponent();
        }
        private void NavigateBackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void DoctorTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void PatientsTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void YesterdayButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void TomorrowButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NewPeriodButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void DeletePeriodButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
