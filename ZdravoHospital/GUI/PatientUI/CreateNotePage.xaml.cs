using Model;
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
    /// Interaction logic for CreateNotePage.xaml
    /// </summary>
    public partial class CreateNotePage : Page
    {
        public string PatientUsername { get; set; }
        public PatientNote PatientNote { get; set; }
        public CreateNotePage(string username)
        {
            InitializeComponent();
            DataContext = this;
            FillProperties(username);
            
        }
        private void FillProperties(string username)
        {
            PatientUsername = username;
            PatientNote = new PatientNote();
            PatientNote.NotifyTime = DateTime.Now;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NotificationsPage(PatientUsername));
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(PatientNote.NotifyTime.ToString()+" razmak"+PatientNote.Content);
            //Model.Resources.patients[PatientUsername].PatientNotes.Add(PatientNote);

        }
    }
}
