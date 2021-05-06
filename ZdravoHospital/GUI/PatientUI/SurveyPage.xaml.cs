using Model;
using System;
using System.Collections.Generic;
using System.Linq;
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

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for SurveyPage.xaml
    /// </summary>
    public partial class SurveyPage : Page
    {
        public Survey Survey { get; set; }
        public PatientWindow PatientWindow { get; set; }
        public SurveyPage(PatientWindow patientWindow)
        {
            InitializeComponent();
            Survey = new Survey();
            DataContext = this;
            Model.Resources.OpenSurveys();
            PatientWindow = patientWindow;

            
        }

        private void submitmButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (!(firstRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) || !(secondRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) 
                || !(thirdRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) || !(fourthRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true))
                )
            {
                CustomOkDialog customOkDialog = new CustomOkDialog("Warning", "Please fill out the survey!");
                customOkDialog.ShowDialog();
                return;
            }
            serializeSurvey();
            CustomOkDialog customOkDialog1 = new CustomOkDialog("Survey", "Thank you for completing survey!");
            customOkDialog1.ShowDialog();
            PatientWindow.SurveyAvailable = false;
            NavigationService.Navigate(new AppointmentPage(PatientWindow.Patient.Username));
        }

        private void cancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage(PatientWindow.Patient.Username));
        }

        public void serializeSurvey()
        {
            Survey.CreationDate = DateTime.Now;
            Survey.PatientUsername = PatientWindow.Patient.Username;
            Model.Resources.surveys.Add(Survey);
            Model.Resources.SaveSurveys();
        }
    }
}
