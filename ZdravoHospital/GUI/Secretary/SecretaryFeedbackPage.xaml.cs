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
using ZdravoHospital.GUI.Secretary.Service;
using ZdravoHospital.GUI.Secretary.ViewModels;

namespace ZdravoHospital.GUI.Secretary
{
    /// <summary>
    /// Interaction logic for SecretaryFeedbackPage.xaml
    /// </summary>
    public partial class SecretaryFeedbackPage : Page
    {
        public int SelectedType { get; set; }
        public string FeedbackComment { get; set; }
        public FeedbackService FeedbackService { get; set; }

        public SecretaryFeedbackPage()
        {
            InitializeComponent();
            this.DataContext = this;
            FeedbackService = new FeedbackService();
        }

        private void SendFeedbackButton_Click(object sender, RoutedEventArgs e)
        {
            if(SelectedType != -1 && FeedbackComment != "") 
            {
                Feedback newFeedback = new Feedback();
                newFeedback.Id = Guid.NewGuid();
                newFeedback.SenderUsername = SecretaryWindowVM.SecretaryUsername;
                newFeedback.Type = (FeedbackType)SelectedType;
                newFeedback.Text = FeedbackComment;
                FeedbackService.AddFeedback(newFeedback);
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Success", "Submitted successfully.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
                NavigationService.Navigate(new SecretaryHomePage());
            }
            else
            {
                SecretaryWindowVM.CustomMessageBox = new CustomMessageBox("Sorry", "All fields are required.");
                SecretaryWindowVM.CustomMessageBox.Owner = SecretaryWindowVM.SecretaryWindow;
                SecretaryWindowVM.CustomMessageBox.Show();
            }
        }
    }
}
