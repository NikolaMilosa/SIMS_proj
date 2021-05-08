using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

namespace ZdravoHospital.GUI.DoctorUI
{
    /// <summary>
    /// Interaction logic for ReferralPage.xaml
    /// </summary>
    public partial class ReferralPage : Page
    {
        public Referral Referral { get; set; }
        public Doctor ReferringDoctor { get; set; }
        public Patient Patient { get; set; }
        public Period Period { get; set; }

        public ObservableCollection<Doctor> Doctors { get; set; }

        public ReferralPage(Doctor referringDoctor, Patient patient, Period period)
        {
            InitializeComponent();

            DataContext = this;
            Model.Resources.OpenReferrals();

            foreach (Referral r in Model.Resources.referrals)
                if (r.ReferralId == period.ReferredReferralId)
                {
                    Referral = r;
                    break;
                }

            ReferringDoctor = referringDoctor;
            Patient = patient;
            Period = period;
            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values.Where(d => !d.Username.Equals(referringDoctor.Username)));

            if (Referral != null)
            {
                DoctorsComboBox.SelectedItem = Model.Resources.doctors[Referral.ReferredDoctorUsername];
                NoteTextBox.Text = Referral.Note;
                DaysToUseTextBox.Text = Referral.DaysToUse.ToString();
                
                if (Referral.IsUsed)
                {
                    DoctorsComboBox.IsHitTestVisible = false;
                    DoctorsComboBox.IsTabStop = false;
                    NoteTextBox.IsReadOnly = true;
                    DaysToUseTextBox.IsReadOnly = true;

                    if (Referral.Period.PeriodType == PeriodType.APPOINTMENT)
                        ReferredAppointmentButton.Visibility = Visibility.Visible;
                    else
                        ReferredOperationButton.Visibility = Visibility.Visible;
                }
                else
                {
                    EditButton.Visibility = Visibility.Visible;
                    UseStackPanel.Visibility = Visibility.Visible;
                }
            }
            else
                ConfirmButton.Visibility = Visibility.Visible;
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (Referral == null)
                return;
            
            if (Referral.IsUsed)
            {
                EditButton.Visibility = Visibility.Collapsed;
                UseStackPanel.Visibility = Visibility.Collapsed;
                DoctorsComboBox.IsHitTestVisible = false;
                DoctorsComboBox.IsTabStop = false;
                NoteTextBox.IsReadOnly = true;
                DaysToUseTextBox.IsReadOnly = true;

                if (Referral.Period.PeriodType == PeriodType.APPOINTMENT)
                    ReferredAppointmentButton.Visibility = Visibility.Visible;
                else
                    ReferredOperationButton.Visibility = Visibility.Visible;
            }
            else
            {
                EditButton.Visibility = Visibility.Visible;
                ReferredAppointmentButton.Visibility = Visibility.Collapsed;
                ReferredAppointmentButton.Visibility = Visibility.Collapsed;
                UseStackPanel.Visibility = Visibility.Visible;
                DoctorsComboBox.IsHitTestVisible = true;
                DoctorsComboBox.IsTabStop = true;
                NoteTextBox.IsReadOnly = false;
                DaysToUseTextBox.IsReadOnly = false;
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            int id = Model.Resources.referrals.Count > 0 ? 
                Model.Resources.referrals.Last().ReferralId + 1 
                : 0;

            Referral = new Referral()
            {
                ReferralId = id,
                ReferringDoctorUsername = ReferringDoctor.Username,
                ReferredDoctorUsername = (DoctorsComboBox.SelectedItem as Doctor).Username,
                Note = NoteTextBox.Text,
                DaysToUse = Int32.Parse(DaysToUseTextBox.Text)
            };
            Model.Resources.referrals.Add(Referral);
            Model.Resources.SaveReferrals();

            Period.ReferredReferralId = Referral.ReferralId;
            Model.Resources.SavePeriods();

            UseStackPanel.Visibility = Visibility.Visible;
            ConfirmButton.Visibility = Visibility.Collapsed;
            EditButton.Visibility = Visibility.Visible;
            DoctorsComboBox.IsHitTestVisible = false;
            DoctorsComboBox.IsTabStop = false;
            NoteTextBox.IsReadOnly = true;
            DaysToUseTextBox.IsReadOnly = true;
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            UseStackPanel.Visibility = Visibility.Collapsed;
            ConfirmButton.Visibility = Visibility.Visible;
            EditButton.Visibility = Visibility.Collapsed;
            DoctorsComboBox.IsHitTestVisible = true;
            DoctorsComboBox.IsTabStop  = true;
            NoteTextBox.IsReadOnly = false;
            DaysToUseTextBox.IsReadOnly = false;
        }

        private void UseReferralAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewAppointmentPage(Referral, Patient));
        }

        private void UseReferralOperationButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new NewOperationPage(Referral, Patient));
        }

        private void ReferredAppointmentButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentPage(Referral.Period));
        }

        private void ReferredOperationButton_Click(object sender, RoutedEventArgs e)
        {
            bool readonlyMode = !Referral.ReferringDoctorUsername.Equals(App.currentUser);
            NavigationService.Navigate(new OperationPage(Referral.Period, readonlyMode));
        }
    }
}
