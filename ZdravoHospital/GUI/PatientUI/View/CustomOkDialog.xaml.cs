using System.Windows;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for customOkDialog.xaml
    /// </summary>
    public partial class CustomOkDialog : Window
    {

        public CustomOkDialog(string title,string content)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new CustomOkDialogVM(title, content);
        }

    }
}
