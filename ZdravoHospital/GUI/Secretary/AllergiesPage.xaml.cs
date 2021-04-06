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

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for AllergiesPage.xaml
    /// </summary>
    public partial class AllergiesPage : Page
    {
        public AllergiesPage(Model.Patient selectedPatient)
        {
            InitializeComponent();
        }

        private void SubmitAllergiesButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
