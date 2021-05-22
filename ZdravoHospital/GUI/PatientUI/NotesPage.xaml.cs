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

        public Patient Patient{ get; set; }

        public PatientRepository PatientRepository { get; set; }

        public NotesPage(string username)
        {
            InitializeComponent();
            FillProperties(username);
            DataContext = this;

        }
        
        private void FillProperties(string username)
        {
            
            PatientRepository = new PatientRepository();
            Patient = PatientRepository.GetById(username);
            ObservableNotes = new ObservableCollection<PatientNote>(Patient.PatientNotes);
        }

        private void NoteButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new CreateNotePage(Patient.Username));
        }

        private void RemoveNote()
        {
            PatientNote note = (PatientNote)NotesDataGrid.SelectedItem;
            Patient.PatientNotes.Remove(note);
            SerializePatient();
            ObservableNotes.Remove(note);
        }

        private void SerializePatient()
        {
            PatientRepository.Update(Patient);
        }
        private void RemoveButton_Click(object sender, RoutedEventArgs e)
        {
            RemoveNote();
            Validate.ShowOkDialog("Removed","Note succesffuly removed!");
        }

        private void NotesDataGrid_MouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            NavigationService.Navigate(new NoteDetailsPage((PatientNote)NotesDataGrid.SelectedItem, Patient.Username));
        }
    }
}
