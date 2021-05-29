using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for NotesPage.xaml
    /// </summary>
    public partial class NotesPage : Page
    {
      

        public NotesPage(string username)
        {
            InitializeComponent();
            DataContext = new NotesPageVM();

        }
        
    }
}
