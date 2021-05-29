using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for PeriodDetailsPage.xaml
    /// </summary>
    public partial class AnamnesisPage : Page
    {
        public AnamnesisPage(string anamnesis)
        {
            InitializeComponent();
            DataContext = new AnamnesisPageVM(anamnesis);
          
        }

    }
}
