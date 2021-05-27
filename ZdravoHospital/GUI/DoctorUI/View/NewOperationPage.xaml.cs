using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for NewOperationPage.xaml
    /// </summary>
    public partial class NewOperationPage : Page
    {

        public Thickness TopPanelMargin { get; set; }

        public NewOperationPage(Doctor doctor, DateTime startTime, int duration)
        {
            InitializeComponent();
            DataContext = new NewOperationViewModel(doctor, startTime, duration);
        }

        public NewOperationPage(Referral referral, Patient patient)
        {
            InitializeComponent();
            DataContext = new NewOperationViewModel(referral, patient);
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void PatientInfoButton_Click(object sender, RoutedEventArgs e)
        {
            Patient patient = PatientsComboBox.SelectedItem as Patient;

            if (patient != null)
                NavigationService.Navigate(new PatientInfoPage(patient));
        }
    }
}
