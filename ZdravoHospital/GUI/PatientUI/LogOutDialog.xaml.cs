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
    /// Interaction logic for LogOutDialog.xaml
    /// </summary>
    public partial class LogOutDialog : Window
    {
        private PatientWindow patientWindow;
        public LogOutDialog(PatientWindow patientWindow)
        {
            InitializeComponent();
            this.patientWindow = patientWindow;
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
        }

        private void Button_Click(object sender, RoutedEventArgs e)//yes button
        {
            MainWindow mainWindow = new MainWindow();
            patientWindow.Patient.RecentActions = PatientWindow.RecentActionsNum;
            Model.Resources.SavePatients();
            mainWindow.Show();
            Close();
            patientWindow.Close();
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)//no button
        {
            Close();
        }
    }
}
