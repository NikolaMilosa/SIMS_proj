using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public ObservableCollection<Doctor> Doctors { get; set; }

        public ReferralPage(Referral referral)
        {
            InitializeComponent();

            DataContext = this;
            Doctors = new ObservableCollection<Doctor>(Model.Resources.doctors.Values);
            Referral = referral;

            if (referral == null)
            {
                Referral = new Referral()
                {
                    IsUsed = false
                };
            }    
            else
            {
                DoctorsComboBox.SelectedItem = Model.Resources.doctors[Referral.ReferredDoctorUsername];
                NoteTextBox.Text = Referral.Note;
                DaysToUseTextBox.Text = Referral.NumberOfDays.ToString();
            }
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.GoBack();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UseReferralAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void UseReferralOperationButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReferredAppointmentButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ReferredOperationButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
