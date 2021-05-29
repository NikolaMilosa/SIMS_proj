using System.Windows;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for CustomYesNoDialog.xaml
    /// </summary>
    public partial class CustomYesNoDialog : Window
    {
       
        public CustomYesNoDialog(string title,string content)
        {
            InitializeComponent();
            WindowStartupLocation = WindowStartupLocation.CenterScreen;
            DataContext = new CustomYesNoDialogVM(title, content, this);
        }

    }
}
