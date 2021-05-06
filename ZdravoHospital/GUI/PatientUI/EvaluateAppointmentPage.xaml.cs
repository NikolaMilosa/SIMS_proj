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
using ZdravoHospital.GUI.PatientUI.ViewModel;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for EvaluateAppointmentPage.xaml
    /// </summary>
    public partial class EvaluateAppointmentPage : Page
    {
        public AppointmentView AppointmentView { get; set; }
        public PeriodMark PeriodMark { get; set; }
        public EvaluateAppointmentPage(AppointmentView appointmentView)
        {
            InitializeComponent();
            DataContext = this;
            AppointmentView = appointmentView;
            if (appointmentView.Period.PeriodMark == null)
            {
                appointmentView.Period.PeriodMark = new PeriodMark();
                appointmentView.Period.PeriodMark.Mark = -1;
            }

            PeriodMark = appointmentView.Period.PeriodMark;
            SetMark();

        }

        private void SetMark()
        {
            switch (PeriodMark.Mark)
            {
                case 1:
                    ButtonStar1_Click(null, null);
                    break;
                case 2:
                    ButtonStar2_Click(null, null);
                    break;
                case 3:
                    ButtonStar3_Click(null, null);
                    break;
                case 4:
                    ButtonStar4_Click(null, null);
                    break;
                case 5:
                    ButtonStar5_Click(null, null);
                    break;
            }
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            CustomOkDialog customOkDialog = null;
            if (PeriodMark.Mark == -1)
            {
                Validations.Validate.ShowOkDialog("Warning", "Please enter your mark for the period!")
                return;
            }
            customOkDialog = new CustomOkDialog("Rated", "Period successfully rated!");
            customOkDialog.ShowDialog();
            Model.Resources.SavePeriods();
            NavigationService.Navigate(new AppointmentHistoryPage(AppointmentView.Period.PatientUsername));
        }

        private bool IsPeriodRated()
        {
            bool unrated = false;

            return unrated;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentHistoryPage(AppointmentView.Period.PatientUsername));
        }

        private void ButtonStar1_Click(object sender, RoutedEventArgs e)
        {
            PeriodMark.Mark = 1;
            firstImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            secondImage.Source= (ImageSource)FindResource(resourceKey: "EmptyStar");
            thirdImage.Source= (ImageSource)FindResource(resourceKey: "EmptyStar");
            fourthImage.Source= (ImageSource)FindResource(resourceKey: "EmptyStar");
            fifthImage.Source=(ImageSource)FindResource(resourceKey: "EmptyStar");
        }

        private void ButtonStar2_Click(object sender, RoutedEventArgs e)
        {
            PeriodMark.Mark = 2;
            firstImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            secondImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            thirdImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
            fourthImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
            fifthImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
        }

        private void ButtonStar3_Click(object sender, RoutedEventArgs e)
        {
            PeriodMark.Mark = 3;
            firstImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            secondImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            thirdImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            fourthImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
            fifthImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
        }

        private void ButtonStar4_Click(object sender, RoutedEventArgs e)
        {
            PeriodMark.Mark = 4;
            firstImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            secondImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            thirdImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            fourthImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            fifthImage.Source = (ImageSource)FindResource(resourceKey: "EmptyStar");
        }

        private void ButtonStar5_Click(object sender, RoutedEventArgs e)
        {
            PeriodMark.Mark = 5;
            firstImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            secondImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            thirdImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            fourthImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
            fifthImage.Source = (ImageSource)FindResource(resourceKey: "FullStar");
        }
    }
}
