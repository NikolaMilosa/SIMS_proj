using System.Windows.Controls;
using ZdravoHospital.GUI.PatientUI.DTOs;
using ZdravoHospital.GUI.PatientUI.ViewModels;

namespace ZdravoHospital.GUI.PatientUI.View
{
    /// <summary>
    /// Interaction logic for NotificationDetailsPage.xaml
    /// </summary>
    public partial class NotificationDetailsPage : Page
    {
        public NotificationDetailsPage(NotificationDTO notificationView)
        {
            InitializeComponent();

            DataContext = new NotificationDetailsPageVM(notificationView);
        }

        
    }
}
