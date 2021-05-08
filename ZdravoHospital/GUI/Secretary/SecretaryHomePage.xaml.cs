using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
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
    /// Interaction logic for SecretaryHomePage.xaml
    /// </summary>
    public partial class SecretaryHomePage : Page
    {
        public ObservableCollection<string> Suggestions { get; set; }

        public SecretaryHomePage()
        {
            InitializeComponent();
            Suggestions = new ObservableCollection<string>();
            Suggestions.Add("New period");
            Suggestions.Add("New account");
            Suggestions.Add("New guest");
            Suggestions.Add("Urgent period");
            Suggestions.Add("New notification");
            this.DataContext = this;

            setSuggestionsFilter();
        }
        private bool SuggestionsFilter(object item)
        {
            if (String.IsNullOrEmpty(SearchTextBox.Text))
                return true;
            else
                return ((item.ToString()).IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0);
        }

        private bool areThereAnySuggestions()
        {
            foreach(string suggestion in Suggestions)
            {
                if (suggestion.IndexOf(SearchTextBox.Text, StringComparison.OrdinalIgnoreCase) >= 0)
                    return true;
            }
            return false;
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (SearchTextBox.Text == "")
                SuggestionsListBox.Visibility = Visibility.Hidden;
            else
                SuggestionsListBox.Visibility = Visibility.Visible;
            SuggestionsListBox.SelectedIndex = -1;

            if (!areThereAnySuggestions())
                SuggestionsListBox.Visibility = Visibility.Hidden;

            CollectionViewSource.GetDefaultView(SuggestionsListBox.ItemsSource).Refresh();
        }
        private void setSuggestionsFilter()
        {
            ICollectionView viewSuggestions = (ICollectionView)CollectionViewSource.GetDefaultView(Suggestions);
            viewSuggestions.Filter = SuggestionsFilter;
        }

        private void SuggestionsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            SuggestionsListBox.Visibility = Visibility.Hidden;
            if(SuggestionsListBox.SelectedItem != null)
                SearchTextBox.Text = SuggestionsListBox.SelectedItem.ToString();
        }

        private void executeSmartSuggestion()
        {
            if (isUrgentSuggestion())
            {
                NavigationService.Navigate(new SecretaryUrgentPeriodPage());
                return;
            }
            else if (isNotificationSuggestion())
            {
                NavigationService.Navigate(new NewNotificationPage());
                return;
            }
            else if (isPeriodSuggestion())
            {
                NavigationService.Navigate(new SecretaryNewPeriodPage());
                return;
            }
            else if (isGuestSuggestion())
            {
                NavigationService.Navigate(new GuestAccountPage(false));
                return;
            }
            else if (isAccountSuggestion())
            { 
                NavigationService.Navigate(new PatientRegistrationPage());
                return;
            }
            else
                MessageBox.Show("Sorry. No suggestions available.");
        }

        private bool isPeriodSuggestion()
        {
            if (SearchTextBox.Text.IndexOf("period", StringComparison.OrdinalIgnoreCase) >= 0 
                || SearchTextBox.Text.IndexOf("appointment", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("operation", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
        private bool isAccountSuggestion()
        {
            if (SearchTextBox.Text.IndexOf("register", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("account", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("patient", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        private bool isGuestSuggestion()
        {
            if (SearchTextBox.Text.IndexOf("guest", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        private bool isUrgentSuggestion()
        {
            if (SearchTextBox.Text.IndexOf("urgent", StringComparison.OrdinalIgnoreCase) >= 0 
                || SearchTextBox.Text.IndexOf("emergency", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }
        private bool isNotificationSuggestion()
        {
            if (SearchTextBox.Text.IndexOf("notification", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("announce", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("notify", StringComparison.OrdinalIgnoreCase) >= 0
                || SearchTextBox.Text.IndexOf("message", StringComparison.OrdinalIgnoreCase) >= 0)
            {
                return true;
            }
            return false;
        }

        private void GoButton_Click(object sender, RoutedEventArgs e)
        {
            executeSmartSuggestion();
        }

        private void EmergencyWidget_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecretaryUrgentPeriodPage());
        }

        private void NewPeriodWidget_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new SecretaryNewPeriodPage());
        }
    }
}
