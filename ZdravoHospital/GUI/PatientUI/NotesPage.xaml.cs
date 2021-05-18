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
using ZdravoHospital.GUI.PatientUI.Validations;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
        public ObservableCollection<PatientNote> ObservableNotes { get; set; }
        public string PatientUsername { get; set; }

        public List<PatientNote> PatientNotes { get; set; }

        public PatientRepository PatientRepository { get; set; }

        public NotesPage(string username)
        {
            InitializeComponent();
            FillProperties(username);
            DataContext = this;

        }
        
        private void FillProperties(string username)
        {
            PatientUsername = username;
            PatientRepository = new PatientRepository();
            PatientNotes = PatientRepository.GetById(username).PatientNotes;
            ObservableNotes = new ObservableCollection<PatientNote>(PatientNotes);
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateNotePage(PatientUsername));
        }

        private void RemoveNote()
        {
            PatientNote note = (PatientNote)NotesDataGrid.SelectedItem;
            PatientNotes.Remove(note);
            ObservableNotes.Remove(note);
        }

        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveNote();
            PatientRepository.Update(null);
            Validate.ShowOkDialog("Removed","Note succesffuly removed!");
        }

        private void NotesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NoteDetailsPage((PatientNote)NotesDataGrid.SelectedItem, PatientUsername));
        }
    }
}
