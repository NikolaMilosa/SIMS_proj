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

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PrescriptionPage.xaml
    /// </summary>
    public partial class PrescriptionPage : Page
    {
        public PrescriptionPage()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void AddTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Visible;
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void CancelTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Hidden;
        }

        private void ConfirmTherapyButton_Click(object sender, RoutedEventArgs e)
        {
            NewTherapyPopup.Visibility = Visibility.Hidden;
        }
    }
}
