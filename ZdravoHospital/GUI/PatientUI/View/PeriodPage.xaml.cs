using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for PeriodPage.xaml
    /// </summary>
    /// 
   
    public partial class PeriodPage : Page
    {

        public PeriodPage(string username)
        {
            InitializeComponent();
            DataContext = new PeriodPageVM(username);
        }


    }
}
