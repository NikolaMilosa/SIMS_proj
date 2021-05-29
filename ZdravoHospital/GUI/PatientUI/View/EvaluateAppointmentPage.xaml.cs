using System.Windows.Controls;
using Model;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for EvaluateAppointmentPage.xaml
    /// </summary>
    public partial class EvaluateAppointmentPage : Page
    {
        public EvaluateAppointmentPage(Period period)
        {
            InitializeComponent();
            DataContext = new EvaluateAppointmentPageVM(period);
           
        }

    }
}
