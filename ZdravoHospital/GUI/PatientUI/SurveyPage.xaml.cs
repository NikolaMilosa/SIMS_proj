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
using ZdravoHospital.GUI.PatientUI.Validations;

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

        private void SubmitmButton_Click(object sender, RoutedEventArgs e)
        {
           
            if (!AreRadioButtonsFilled())
                return;

            SurveySuccesfullyCompleted();
            SerializeSurvey();
            NavigationService.Navigate(new AppointmentPage(PatientWindow.PatientUsername));
        }

        private void SurveySuccesfullyCompleted()
        {
            PatientWindow.SurveyAvailable = false;
            Validate.ShowOkDialog("Survey", "Thank you for completing survey!");
        }

        private bool AreRadioButtonsFilled()
        {
            bool filled = true;
            if (!(firstRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) || !(secondRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) || !(thirdRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)) || !(fourthRadioButtonPanel.Children.OfType<RadioButton>().Any(rb => rb.IsChecked == true)))
            {
                filled = false;
                Validate.ShowOkDialog("Warning", "Please fill out the survey!");
            }
           
           return filled;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage(PatientWindow.PatientUsername));
        }

        public void SerializeSurvey()
        {
            Survey.CreationDate = DateTime.Now;
            Survey.PatientUsername = PatientWindow.PatientUsername;
            Model.Resources.surveys.Add(Survey);
            Model.Resources.SaveSurveys();
        }
    }
}
