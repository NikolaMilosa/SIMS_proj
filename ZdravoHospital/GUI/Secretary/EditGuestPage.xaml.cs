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
    /// Interaction logic for EditGuestPage.xaml
    /// </summary>
    public partial class EditGuestPage : Page
    {
        public Patient SelectedPatient { get; set; }
        public GuestDTO Guest { get; set; }
        public GuestService GuestService { get; set; }

        public EditGuestPage(Patient selectedPatient)
        {
            InitializeComponent();
            SelectedPatient = selectedPatient;
            GuestService = new GuestService();
            Guest = new GuestDTO(SelectedPatient.Name, SelectedPatient.Surname, SelectedPatient.CitizenId, SelectedPatient.HealthCardNumber);
            this.DataContext = this;
        }

        private void FinishButton_Click(object sender, RoutedEventArgs e)
        {
            GuestService.ProcessGuestUpdate(Guest);
            NavigationService.Navigate(new PatientsView());
        }

    }
}
