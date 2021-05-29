using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for NotificationsPage.xaml
    /// </summary>
    public partial class NotificationsPage : Page
    {

        public NotificationsPage(string username)
        {
            InitializeComponent();
            DataContext = new NotificationsPageVM();
        }

     

    }
}
