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

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for RemoveAppointmentDialog.xaml
    /// </summary>
    public partial class RemoveAppointmentDialog : Window
    {
        public static bool YesPressed { get; set; }
        public RemoveAppointmentDialog()
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void yesButton_Click(object sender, RoutedEventArgs e)
        {
            YesPressed = true;
            Close();
        }

        private void noButton_Click(object sender, RoutedEventArgs e)
        {
            YesPressed = false;
            Close();
        }
    }
}
