using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using Period = Model.Period;

namespace ZdravoHospital.GUI.PatientUI.View
{
    public partial class AddAppointmentPage : Page
    {
        public AddAppointmentPage(Period period)
        {
            InitializeComponent();
            DataContext = period==null ? new AddAppointmentPageVM() : new AddAppointmentPageVM(period);
        }

    }
}
