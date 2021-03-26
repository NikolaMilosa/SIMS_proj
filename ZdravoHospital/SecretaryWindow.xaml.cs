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
using System.Windows.Shapes;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for SecretaryWindow.xaml
    /// </summary>
    public partial class SecretaryWindow : Window
    {
        private PatientRegistrationPage _patientRegPage;

        public SecretaryWindow()
        {
            InitializeComponent();
            _patientRegPage = new PatientRegistrationPage();
            this.SecretaryMainFrame.Content = new SecretaryHomePage();
        }

    }
}
