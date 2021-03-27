using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ZdravoHospital
{
    /// <summary>
    /// Interaction logic for SchedulePage.xaml
    /// </summary>
    public partial class SchedulePage : Page
    {
        public SchedulePage()
        {
            InitializeComponent();
        }

        private void NewAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage());
        }

        private void NewOperationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new OperationPage());
        }
    }
}
