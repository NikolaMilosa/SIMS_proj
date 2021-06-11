using System;
using System.Collections.Generic;
using System.Configuration;
using System.Text;
using System.Windows;
using System.Windows.Navigation;
using Model;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.Logics;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class EvaluateAppointmentPageVM : ViewModel
    {
        #region AdditionalClass

        public class StringHolder : ViewModel
        {
            private string str;
            public string String
            {
                get => str;
                set
                {
                    str = value;
                    OnPropertyChanged("String");
                }
            }

            public StringHolder(string s) => String = s;
        }


        #endregion

        #region Properties

        public Period Period { get; private set; }
        public PeriodMark PeriodMark { get; set; }
        public ViewService ViewFunctions { get; private set; }

        private StringHolder imageSource1;

        public StringHolder ImageSource1
        {
            get
            {
                if(imageSource1==null)
                    imageSource1 = new StringHolder("/Images/PatientUI/emptyStar.png");
                return imageSource1;
            } 
            set
            {
                imageSource1 = value;
                OnPropertyChanged("ImageSource1");
            }
        }
        private StringHolder imageSource2;

        public StringHolder ImageSource2
        {
            get
            {
                if (imageSource2 == null)
                    imageSource2 = new StringHolder("/Images/PatientUI/emptyStar.png");
                return imageSource2;
            }
            set
            {
                imageSource2 = value;
                OnPropertyChanged("ImageSource2");
            }
        }
        private StringHolder imageSource3;

        public StringHolder ImageSource3
        {
            get
            {
                if (imageSource3 == null)
                    imageSource3 = new StringHolder("/Images/PatientUI/emptyStar.png");
                return imageSource3;
            }
            set
            {
                imageSource3 = value;
                OnPropertyChanged("ImageSource3");
            }
        }
        private StringHolder imageSource4;

        public StringHolder ImageSource4
        {
            get
            {
                if (imageSource4 == null)
                    imageSource4 = new StringHolder("/Images/PatientUI/emptyStar.png");
                return imageSource4;
            }
            set
            {
                imageSource4 = value;
                OnPropertyChanged("ImageSource4");
            }
        }
        private StringHolder imageSource5;

        public StringHolder ImageSource5
        {
            get
            {
                if (imageSource5 == null)
                    imageSource5=new StringHolder("/Images/PatientUI/emptyStar.png");

                return imageSource5;
            }
            set
            {
                imageSource5 = value;
                OnPropertyChanged("ImageSource5");
            }
        }



        public List<StringHolder> Images { get; private set; }

        #endregion

        #region Constructors

        public EvaluateAppointmentPageVM(Period period)
        {
            SetProperties(period);
            SetCommands();
        }


        #endregion

        #region Commands

        public RelayCommand StarClickedCommand { get; private set; }
        public RelayCommand ConfirmCommand { get; private set; }
        public RelayCommand CancelCommand { get; private set; }
        #endregion

        #region CommandActions

        public void StarClickedExecute(object parameter)
        {
            int starNum= Int32.Parse((string)parameter);
            PeriodMark.Mark = starNum;
            for (int i = 0; i < starNum; i++)//oboj zvezdice u zutu
                Images[i].String = @"\Images\PatientUI\fullStar.png";

            for (int i = 4; i >= starNum; i--)//ostalima skini boju
                Images[i].String = @"\Images\PatientUI\emptyStar.png";
        }

        public void ConfirmExecute(object parameter)
        {
            PeriodService periodFunctions = new PeriodService();
            periodFunctions.UpdatePeriod(Period);
            ViewService viewFunctions = new ViewService();
            viewFunctions.ShowOkDialog("Rated", "Period successfully rated!");
            PatientWindowVM.NavigationService.Navigate(new AppointmentHistoryPage(Period.PatientUsername));
        }

        public bool ConfirmCanExecute(object parameter)
        {
            return PeriodMark.Mark != 0;
        }

        public void CancelExecute(object parameter)
        {
            PatientWindowVM.NavigationService.Navigate(new AppointmentHistoryPage(Period.PatientUsername));
        }
        #endregion

        #region Methods

        private void SetCommands()
        {
            StarClickedCommand = new RelayCommand(StarClickedExecute);
            ConfirmCommand = new RelayCommand(ConfirmExecute, ConfirmCanExecute);
            CancelCommand = new RelayCommand(CancelExecute);
        }

        private void SetProperties(Period period)
        {
            Period = period;
            ViewFunctions = new ViewService();
            GeneratePeriodMark(period);
            SetImagesList();
        }

        private void SetImagesList()
        {
            Images = new List<StringHolder>();
            Images.Add(ImageSource1);
            Images.Add(ImageSource2);
            Images.Add(ImageSource3);
            Images.Add(ImageSource4);
            Images.Add(ImageSource5);
            StarClickedExecute(PeriodMark.Mark.ToString());
        }
        private void GeneratePeriodMark(Period period)
        {
            if (period.PeriodMark == null)
            {
                period.PeriodMark = new PeriodMark();
                period.PeriodMark.Mark = 0;
            }

            PeriodMark = period.PeriodMark;
        }

        #endregion

    }
}
