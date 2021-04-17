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

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for PeriodDetailsPage.xaml
    /// </summary>
    public partial class AnamnesisPage : Page
    {
        public string AnamnesisContent { get; set; }

        public string Username { get; set; }
        public AnamnesisPage(string anamnesis,string username)
        {
            InitializeComponent();
            DataContext = this;
            AnamnesisContent = anamnesis;
            Username = username;
        }

        private void backButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentHistoryPage(Username));
        }
    }
}
