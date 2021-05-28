using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for SurveyPage.xaml
    /// </summary>
    public partial class SurveyPage : Page
    {
      
        public SurveyPage(PatientWindowVM patientWindowVm)
        {
            InitializeComponent();
            DataContext = new SurveyPageVM(patientWindowVm);
        }

    }
}
