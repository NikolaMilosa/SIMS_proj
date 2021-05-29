using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for SuggestAppointmentPage.xaml
    /// </summary>
    public partial class SuggestAppointmentPage : Page
    {
        public SuggestAppointmentPage()
        {
            InitializeComponent();
            DataContext = new SuggestAppointPageVM();
        }
    }
}
