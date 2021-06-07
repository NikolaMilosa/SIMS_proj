using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using System.Windows.Navigation;
using ZdravoHospital.GUI.Secretary.Commands;

namespace ZdravoHospital.GUI.Secretary.ViewModels
{
    public class SecretaryWindowVM
    {
        public static string SecretaryUsername;
        public static SecretaryWindow SecretaryWindow;
        public static CustomMessageBox CustomMessageBox;
        public static CustomYesNoDialog CustomYesNoDialog;
        public static NavigationService NavigationService { get; set; }
        public SecretaryWindowVM(string username, SecretaryWindow secretaryWindow)
        {
            SecretaryUsername = username;
            SecretaryWindow = secretaryWindow;
            CustomMessageBox = new CustomMessageBox("", "");
            CustomYesNoDialog = new CustomYesNoDialog("", "");
            //CustomMessageBox.Owner = SecretaryWindow;
            SecretaryWindow.SecretaryMainFrame.Content = new SecretaryHomePage();
            NavigationService = SecretaryWindow.SecretaryMainFrame.NavigationService;
            initializeCommands();
        }

        #region Commands
        public ICommand HomePageCommand { get; set; }
        public ICommand PatientsPageCommand { get; set; }
        public ICommand PeriodsPageCommand { get; set; }
        public ICommand NewAccountPageCommand { get; set; }
        public ICommand UrgentPeriodPageCommand { get; set; }
        public ICommand GuestPageCommand { get; set; }
        public ICommand NotificationsPageCommand { get; set; }
        public ICommand DoctorsPageCommand { get; set; }
        public ICommand DemoPageCommand { get; set; }
        public ICommand HelpCommand { get; set; }
        public ICommand AboutCommand { get; set; }
        public ICommand LogoutCommand { get; set; }
        public ICommand ReportPageCommand { get; set; }
        public ICommand FeedbackCommand { get; set; }

        private void initializeCommands()
        {
            HomePageCommand = new RelayCommand(homeExecute);
            PatientsPageCommand = new RelayCommand(patientsViewExecute);
            PeriodsPageCommand = new RelayCommand(periodsExecute);
            NewAccountPageCommand = new RelayCommand(newAccountExecute);
            UrgentPeriodPageCommand = new RelayCommand(urgentPeriodExecute);
            GuestPageCommand = new RelayCommand(guestAccountExecute);
            NotificationsPageCommand = new RelayCommand(notificationsExecute);
            DoctorsPageCommand = new RelayCommand(doctorsViewExecute);
            DemoPageCommand = new RelayCommand(demoExecute);
            HelpCommand = new RelayCommand(helpExecute);
            AboutCommand = new RelayCommand(aboutExecute);
            LogoutCommand = new RelayCommand(logoutExecute);
            ReportPageCommand = new RelayCommand(reportExecute);
            FeedbackCommand = new RelayCommand(feedbackExecute);
        }

        private void feedbackExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryFeedbackPage());
        }
        private void reportExecute(object sender)
        {
            NavigationService.Navigate(new WeeklyReport());
        }
        private void helpExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryHelpPage());
        }
        private void aboutExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryAboutPage());
        }
        private void logoutExecute(object sender)
        {
            SecretaryWindowVM.CustomYesNoDialog = new CustomYesNoDialog("Logging out", "Are you sure?");
            SecretaryWindowVM.CustomYesNoDialog.Owner = SecretaryWindowVM.SecretaryWindow;

            if ((bool)SecretaryWindowVM.CustomYesNoDialog.ShowDialog())
            {
                MainWindow mainWindow = new MainWindow();
                mainWindow.WindowStartupLocation = System.Windows.WindowStartupLocation.CenterScreen;
                SecretaryWindow.Close();
                mainWindow.Show();
            }
            
            
        }

        private void newAccountExecute(object sender)
        {
            NavigationService.Navigate(new PatientRegistrationPage());
        }


        private void guestAccountExecute(object sender)
        {
            NavigationService.Navigate(new GuestAccountPage(false));
        }

        private void notificationsExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryNotificationsPage());
        }


        private void urgentPeriodExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryUrgentPeriodPage());
        }

        private void homeExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryHomePage());
        }

        private void patientsViewExecute(object sender)
        {
            NavigationService.Navigate(new PatientsView());
        }

        private void periodsExecute(object sender)
        {
            NavigationService.Navigate(new SecretaryPeriodsPage());
        }


        private void demoExecute(object sender)
        {
            NavigationService.Navigate(new DemoPage());
        }

        private void doctorsViewExecute(object sender)
        {
            NavigationService.Navigate(new DoctorsView());
        }

        #endregion
    }
}
