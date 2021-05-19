using Model;
using Model.Repository;
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
using ZdravoHospital.GUI.PatientUI.Validations;

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
            NavigationService.Navigate(new NotesPage(PatientUsername));
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (!IsFormFilled())
                return;

            AddNoteToPatient();
            NavigationService.Navigate(new NotesPage(PatientUsername));
        }

        private void AddNoteToPatient()
        {
            PatientRepository patientRepository = new PatientRepository();
            patientRepository.GetById(PatientUsername).PatientNotes.Add(PatientNote);
            patientRepository.Update(null);
        }

        private bool IsFormFilled()
        {
            if (ContentTextBox.Text == null || PatientNote.NotifyTime < DateTime.Now || TitleTextBox.Text==null)
            {
                Validate.ShowOkDialog("Warning","Fill out the form!");
                return false;
            }

            return true;
        }
    }
}
