using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Windows;
using Microsoft.Xaml.Behaviors.Media;
using Syncfusion.Licensing;
using ZdravoHospital.GUI.PatientUI.Commands;
using ZdravoHospital.GUI.PatientUI.View;

namespace ZdravoHospital.GUI.PatientUI.ViewModels
{
    public class WizardWindowVM : ViewModel
    {
        #region Properties


        private string _currentImageSource;
        public string CurrentImageSource
        {
            get => _currentImageSource;
            set
            {
                _currentImageSource = value;
                OnPropertyChanged("CurrentImageSource");
            }
        }
        public int PageCounter { get; private set; }
        private bool _previousButtonVisibility;
        public bool PreviousButtonVisibilty
        {
            get => _previousButtonVisibility;
            set
            {
                _previousButtonVisibility = value;
                OnPropertyChanged("PreviousButtonVisibilty");
            }
        }
        public List<string> ImageSources { get; private set; }

        public List<string> TitleSources { get; private set; }


        private string _nextBtnContent;

        public string NextButtonContent
        {
            get => _nextBtnContent;
            set
            {
                _nextBtnContent = value;
                OnPropertyChanged("NextButtonContent");
            }
        }

        private string _title;

        public string WizardTitle
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged("WizardTitle");
            }
        }

        #endregion

        #region Fields

        private string patientUsername;

        #endregion

        #region Constructors

        public WizardWindowVM(string username)
        {
            patientUsername = username;
            SetProperties();
            SetCommads();
        }

        #endregion

        #region Commands

        public RelayCommand PreviousCommand { get; private set; }
        public RelayCommand NextCommand { get; private set; }
        public RelayCommand SkipCommand { get; private set; }

        #endregion

        #region CommandActions

        public void NextExecute(object parameter)
        {
            if (PageCounter == 5)
                SkipExecute(parameter);
            else
            {
                PageCounter++;
                SetButtons();
                SetCurrentSource();
            }
        }

        public void PreviousExecute(object parameter)
        {
            PageCounter--;
            SetButtons();
            SetCurrentSource();
        }

        public void SkipExecute(object parameter)
        {
            PatientWindow patientWindow = new PatientWindow(patientUsername);
            patientWindow.Show();
            Window window = (Window) parameter;
            window.Close();
        }

        #endregion

        #region Methods

        private void SetCommads()
        {
            NextCommand = new RelayCommand(NextExecute);
            PreviousCommand = new RelayCommand(PreviousExecute);
            SkipCommand = new RelayCommand(SkipExecute);
        }

        private void SetProperties()
        {
            PageCounter = 0;
            SetButtons();
            SetSourceList();
            SetCurrentSource();
        }

        private void SetCurrentSource()
        {
            CurrentImageSource = ImageSources[PageCounter];
            WizardTitle = TitleSources[PageCounter];
        }

        private void SetButtons()
        {
            switch (PageCounter)
            {
                case 0:
                    PreviousButtonVisibilty = false;
                    NextButtonContent = "Next";
                    break;
                case 5:
                    PreviousButtonVisibilty = true;
                    NextButtonContent = "End";
                    break;
                default:
                    PreviousButtonVisibilty = true;
                    NextButtonContent = "Next";
                    break;
            }
        }

        private void SetSourceList()
        {
            ImageSources = new List<string>();
            TitleSources = new List<string>();
            SetImages();
            SetTitles();
        }

        private void SetTitles()
        {
            TitleSources.Add("Working with appointments");
            TitleSources.Add("Working with appointments");
            TitleSources.Add("Working with appointments");
            TitleSources.Add("Working with therapies");
            TitleSources.Add("Other functionallities");
            TitleSources.Add("The End");
        }

        private void SetImages()
        {
            ImageSources.Add(@"/Images/PatientUI/wizard1.png");
            ImageSources.Add(@"/Images/PatientUI/wizard2.png");
            ImageSources.Add(@"/Images/PatientUI/wizard30.png");
            ImageSources.Add(@"/Images/PatientUI/wizard3.png");
            ImageSources.Add(@"/Images/PatientUI/wizard4.png");
            ImageSources.Add(@"/Images/PatientUI/wizard5.png");
        }

        #endregion

    }
}
