﻿using Model;
using System.Windows;
using System.Windows.Controls;
using ZdravoHospital.GUI.DoctorUI.ViewModel;

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for PeriodPage.xaml
    /// </summary>
    public partial class AppointmentPage : Page
    {
        private Period _period;

        public AppointmentPage(Period period)
        {
            InitializeComponent();
            _period = period;
        }
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = new AppointmentViewModel(NavigationService, _period);
        }

        private void PageSizeChanged(object sender, SizeChangedEventArgs e)
        {
            TopDockPanel.Margin = new Thickness(this.ActualWidth * 0.1, 0, this.ActualWidth * 0.1, 15);
        }

        private void SeeReferralButton_Click(object sender, RoutedEventArgs e)
        {
            //Model.Resources.OpenReferrals();
            //Referral referral = Model.Resources.referrals.Find(r => r.ReferralId == period.ReferringReferralId);
            //Patient patient = PatientsComboBox.SelectedItem as Patient;
            //NavigationService.Navigate(new ReferralPage(referral, patient));
        }
    }
}
