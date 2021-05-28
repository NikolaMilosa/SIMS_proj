using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for NoteDetailsPage.xaml
    /// </summary>
    public partial class NoteDetailsPage : Page
    {

        public NoteDetailsPage(PatientNote patientNote)
        {
            InitializeComponent();
            DataContext = new NoteDetailsPageVM(patientNote);
        }

     
    }
}
