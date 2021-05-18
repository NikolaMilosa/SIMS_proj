using Model;
using Model.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
        public ObservableCollection<PatientNote> Notes{ get; set; }
        public string PatientUsername { get; set; }
        

        public NotesPage(string username)
        {
            InitializeComponent();
            FillProperties(username);
            DataContext = this;

        }
        
        private void FillProperties(string username)
        {
            PatientUsername = username;
            PatientRepository patientRepository = new PatientRepository();
            Notes = new ObservableCollection<PatientNote>(patientRepository.GetById(username).PatientNotes);
        }


        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateNotePage(PatientUsername));
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
