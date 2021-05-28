using Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
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
using ZdravoHospital.GUI.Secretary.DTOs;
using ZdravoHospital.GUI.Secretary.Service;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for GuestAccountPage.xaml
    /// </summary>
    public partial class GuestAccountPage : Page
    {
        public GuestDTO Guest { get; set; }
        public GuestService GuestService {get;set;}
        public GuestAccountPage(bool urgentlyCreated)
        {
            Guest = new GuestDTO(urgentlyCreated);
            GuestService = new GuestService();
            InitializeComponent();
            this.DataContext = this;
        }

        public bool isHealthCardUnique(Dictionary<string, Patient> patients, string healthCardNum)
        {
            foreach (KeyValuePair<string, Patient> item in patients)
            {
                if (item.Value.HealthCardNumber.Equals(healthCardNum))
                {
                    return false;
                }
            }
            return true;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            bool success = GuestService.ProcessGuestCreation(Guest);
            if (success)
            {
                if (Guest.UrgentlyCreated)
                    NavigationService.Navigate(new SecretaryUrgentPeriodPage());
                else
                    NavigationService.Navigate(new PatientsView());
            }
            else
            {
                MessageBox.Show("Health card number already exists.");
            }

        }

    }
}
