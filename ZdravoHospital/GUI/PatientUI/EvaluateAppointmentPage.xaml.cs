using Model;
using Model.Repository;
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
using ZdravoHospital.GUI.PatientUI.DTOs;
using PeriodDTO = ZdravoHospital.GUI.PatientUI.DTOs.PeriodDTO;

namespace ZdravoHospital.GUI.PatientUI
{
    /// <summary>
    /// Interaction logic for EvaluateAppointmentPage.xaml
    /// </summary>
    public partial class EvaluateAppointmentPage : Page
    {
        public Period Period { get; set; }
        public PeriodMark PeriodMark { get; set; }
        public EvaluateAppointmentPage(Period period)
        {
            InitializeComponent();
            DataContext = this;
            Period = period;
            GeneratePeriodMark(period);
            SetMark();
        }

        public void GeneratePeriodMark(Period period)
        {
            if (period.PeriodMark == null)
            {
                period.PeriodMark = new PeriodMark();
                period.PeriodMark.Mark = -1;
            }

            PeriodMark = period.PeriodMark;
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
            if (!IsPeriodRated())
                return;

            PeriodSuccessfulyRated();
            NavigationService.Navigate(new AppointmentHistoryPage(Period.PatientUsername));
        }

        private void PeriodSuccessfulyRated()
        {
            PeriodRepository periodRepository = new PeriodRepository();
            periodRepository.Update(Period);
            Validations.Validate.ShowOkDialog("Rated", "Period successfully rated!");
        }

        private bool IsPeriodRated()
        {
            bool rated = true;
            if(PeriodMark.Mark==-1)
            {
                Validations.Validate.ShowOkDialog("Warning", "Please enter your mark for the period!");
                rated = false;
            }
            return rated;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new AppointmentHistoryPage(Period.PatientUsername));
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
