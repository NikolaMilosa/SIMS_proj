using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for CreateNotePage.xaml
    /// </summary>
    public partial class CreateNotePage : Page
    {
        public CreateNotePage()
        {
            InitializeComponent();
            DataContext = new CreateNotePageVM();
        }
    }
}
