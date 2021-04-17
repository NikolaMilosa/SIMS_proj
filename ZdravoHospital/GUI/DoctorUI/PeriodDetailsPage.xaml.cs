using Model;
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
    /// Interaction logic for PeriodDetailsPage.xaml
    /// </summary>
    public partial class PeriodDetailsPage : Page
    {
        private Period period;

        public PeriodDetailsPage(Period period)
        {
            InitializeComponent();

            this.period = period;

            if (period.Details != null)
                DetailsTextBox.Text = period.Details;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.period.Details != null && !this.period.Details.Equals("") && !DetailsTextBox.Text.Equals(this.period.Details))
            {
                MessageBoxResult result = MessageBox.Show("Changes detected. Are you sure you want to overwrite current data?",
                                                          "Warning", MessageBoxButton.YesNo, MessageBoxImage.Warning);

                if (result == MessageBoxResult.No)
                    return;
            }

            this.period.Details = DetailsTextBox.Text;
            Model.Resources.SavePeriods();

            MessageBox.Show("Successfully saved", "Success");

            NavigationService.GoBack();
        }
    }
}
