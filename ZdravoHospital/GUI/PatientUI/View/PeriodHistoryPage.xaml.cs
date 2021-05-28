using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for AppointmentHistoryPage.xaml
    /// </summary>
    public partial class AppointmentHistoryPage : Page
    {
        public AppointmentHistoryPage(string username)
        {
            InitializeComponent();
            DataContext = new PeriodHistoryPageVM();
        }

    }
}
