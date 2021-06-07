using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
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
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for PatientsView.xaml
    /// </summary>
    public partial class PatientsView : Page
    {

        public PatientsView()
        {
            InitializeComponent();
            DataContext = new PatientsViewVM();
        }


        private void UnblockButton_Click(object sender, RoutedEventArgs e)
        {
            var patientToUnblock = (sender as Button).DataContext as Patient;
            new PatientGeneralService().ProcessPatientUnblock(patientToUnblock);
            CollectionViewSource.GetDefaultView(PatientsListView.ItemsSource).Refresh();
        }

    }
}
