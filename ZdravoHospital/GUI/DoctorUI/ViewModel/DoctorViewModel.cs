using Model;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.ViewModel
{
    public class DoctorViewModel : ViewModel
    {
        private Dictionary<string, Uri> _videos = new Dictionary<string, Uri>()
        {
            { "Appointment/operation preview", new Uri("Resources/appointment_operation_preview.mp4", UriKind.Relative) },
            { "Creating new appointment", new Uri("Resources/create_new_appointment.mkv", UriKind.Relative) }
        };

        private MediaElement _mediaElement;
        private bool _isMediaPlaying;
        private FeedbackService _feedbackService;


        public int SelectedFeedbackIndex { get; set; }
        public string FeedbackText { get; set; }

        private string _messageText;
        public string MessageText
        {
            get
            {
                return _messageText;
            }
            set
            {
                _messageText = value;
                OnPropertyChanged("MessageText");
            }
        }

        private Visibility _messagePopUpVisibility;
        public Visibility MessagePopUpVisibility
        {
            get
            {
                return _messagePopUpVisibility;
            }
            set
            {
                _messagePopUpVisibility = value;
                OnPropertyChanged("MessagePopUpVisibility");
            }
        }
        private string _userText;
        public string UserText 
        {
            get => _userText;
            set
            {
                _userText = value;
                OnPropertyChanged("UserText");
            }
        }

        private bool _scheduleTabSelected;
        public bool ScheduleTabSelected
        {
            get => _scheduleTabSelected;
            set
            {
                _scheduleTabSelected = value;
                OnPropertyChanged("ScheduleTabSelected");
            }
        }

        private bool _patientsTabSelected;
        public bool PatientsTabSelected
        {
            get => _patientsTabSelected;
            set
            {
                _patientsTabSelected = value;
                OnPropertyChanged("PatientsTabSelected");
            }
        }

        private bool _medicinesTabSelected;
        public bool MedicinesTabSelected
        {
            get => _medicinesTabSelected;
            set
            {
                _medicinesTabSelected = value;
                OnPropertyChanged("MedicinesTabSelected");
            }
        }

        private bool _notificationsTabSelected;
        public bool NotificationsTabSelected
        {
            get => _notificationsTabSelected;
            set
            {
                _notificationsTabSelected = value;
                OnPropertyChanged("NotificationsTabSelected");
            }
        }

        private BitmapImage _scheduleTabImageSource;
        public BitmapImage ScheduleTabImageSource
        {
            get => _scheduleTabImageSource;
            set
            {
                _scheduleTabImageSource = value;
                OnPropertyChanged("ScheduleTabImageSource");
            }
        }

        private BitmapImage _patientsTabImageSource;
        public BitmapImage PatientsTabImageSource
        {
            get => _patientsTabImageSource;
            set
            {
                _patientsTabImageSource = value;
                OnPropertyChanged("PatientsTabImageSource");
            }
        }

        private BitmapImage _medicinesTabImageSource;
        public BitmapImage MedicinesTabImageSource
        {
            get => _medicinesTabImageSource;
            set
            {
                _medicinesTabImageSource = value;
                OnPropertyChanged("MedicinesTabImageSource");
            }
        }

        private BitmapImage _notificationsTabImageSource;
        public BitmapImage NotificationsTabImageSource
        {
            get => _notificationsTabImageSource;
            set
            {
                _notificationsTabImageSource = value;
                OnPropertyChanged("NotificationsTabImageSource");
            }
        }

        private Brush _scheduleTabTextBlockForeground;
        public Brush ScheduleTabTextBlockForeground
        {
            get => _scheduleTabTextBlockForeground;
            set
            {
                _scheduleTabTextBlockForeground = value;
                OnPropertyChanged("ScheduleTabTextBlockForeground");
            }
        }

        private Brush _patientsTabTextBlockForeground;
        public Brush PatientsTabTextBlockForeground
        {
            get => _patientsTabTextBlockForeground;
            set
            {
                _patientsTabTextBlockForeground = value;
                OnPropertyChanged("PatientsTabTextBlockForeground");
            }
        }

        private Brush _medicinesTabTextBlockForeground;
        public Brush MedicinesTabTextBlockForeground
        {
            get => _medicinesTabTextBlockForeground;
            set
            {
                _medicinesTabTextBlockForeground = value;
                OnPropertyChanged("MedicinesTabTextBlockForeground");
            }
        }

        private Brush _notificationsTabTextBlockForeground;
        public Brush NotificationsTabTextBlockForeground
        {
            get => _notificationsTabTextBlockForeground;
            set
            {
                _notificationsTabTextBlockForeground = value;
                OnPropertyChanged("NotificationsTabTextBlockForeground");
            }
        }

        private Visibility _userPopUpVisibility;
        public Visibility UserPopUpVisibility
        {
            get
            {
                return _userPopUpVisibility;
            }
            set
            {
                _userPopUpVisibility = value;
                OnPropertyChanged("UserPopUpVisibility");
            }
        }

        private Visibility _tutorialPopUpVisibility;
        public Visibility TutorialPopUpVisibility
        {
            get
            {
                return _tutorialPopUpVisibility;
            }
            set
            {
                _tutorialPopUpVisibility = value;
                OnPropertyChanged("TutorialPopUpVisibility");
            }
        }

        private Visibility _feedbackPopUpVisibility;
        public Visibility FeedbackPopUpVisibility
        {
            get
            {
                return _feedbackPopUpVisibility;
            }
            set
            {
                _feedbackPopUpVisibility = value;
                OnPropertyChanged("FeedbackPopUpVisibility");
            }
        }

        private Uri _mediaElementSource;
        public Uri MediaElementSource
        {
            get => _mediaElementSource;
            set
            {
                _mediaElementSource = value;
                OnPropertyChanged("MediaElementSource");
            }
        }

        private string _playPauseButtonText;
        public string PlayPauseButtonText
        {
            get => _playPauseButtonText;
            set
            {
                _playPauseButtonText = value;
                OnPropertyChanged("PlayPauseButtonText");
            }
        }

        private string _mediaTitle;
        public string MediaTitle
        {
            get => _mediaTitle;
            set
            {
                _mediaTitle = value;
                OnPropertyChanged("MediaTitle");
            }
        }

        public MyICommand ScheduleTabCommand { get; set; }

        public void Executed_ScheduleTabCommand()
        {
            if (PatientsTabSelected)
            {
                PatientsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/patients_light_gray.png", UriKind.Relative));
                PatientsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (MedicinesTabSelected)
            {
                MedicinesTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/medicines_light_gray.png", UriKind.Relative));
                MedicinesTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (NotificationsTabSelected)
            {
                NotificationsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/notifications_light_gray.png", UriKind.Relative));
                NotificationsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }

            ScheduleTabSelected = true;
            ScheduleTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/schedule_blue.png", UriKind.Relative));
            ScheduleTabTextBlockForeground = Application.Current.Resources["Blue"] as Brush;
        }

        public bool CanExecute_ScheduleTabCommand()
        {
            return true;
        }

        public MyICommand PatientsTabCommand { get; set; }

        public void Executed_PatientsTabCommand()
        {
            if (ScheduleTabSelected)
            {
                ScheduleTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/schedule_light_gray.png", UriKind.Relative));
                ScheduleTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (MedicinesTabSelected)
            {
                MedicinesTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/medicines_light_gray.png", UriKind.Relative));
                MedicinesTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (NotificationsTabSelected)
            {
                NotificationsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/notifications_light_gray.png", UriKind.Relative));
                NotificationsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }

            PatientsTabSelected = true;
            PatientsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/patients_blue.png", UriKind.Relative));
            PatientsTabTextBlockForeground = Application.Current.Resources["Blue"] as Brush;
        }

        public bool CanExecute_PatientsTabCommand()
        {
            return true;
        }

        public MyICommand MedicinesTabCommand { get; set; }

        public void Executed_MedicinesTabCommand()
        {
            if (ScheduleTabSelected)
            {
                ScheduleTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/schedule_light_gray.png", UriKind.Relative));
                ScheduleTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (PatientsTabSelected)
            {
                PatientsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/patients_light_gray.png", UriKind.Relative));
                PatientsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (NotificationsTabSelected)
            {
                NotificationsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/notifications_light_gray.png", UriKind.Relative));
                NotificationsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }

            MedicinesTabSelected = true;
            MedicinesTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/medicines_blue.png", UriKind.Relative));
            MedicinesTabTextBlockForeground = Application.Current.Resources["Blue"] as Brush;
        }

        public bool CanExecute_MedicinesTabCommand()
        {
            return true;
        }

        public MyICommand NotificationsTabCommand { get; set; }

        public void Executed_NotificationsTabCommand()
        {
            if (ScheduleTabSelected)
            {
                ScheduleTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/schedule_light_gray.png", UriKind.Relative));
                ScheduleTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (PatientsTabSelected)
            {
                PatientsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/patients_light_gray.png", UriKind.Relative));
                PatientsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }
            else if (MedicinesTabSelected)
            {
                MedicinesTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/medicines_light_gray.png", UriKind.Relative));
                MedicinesTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            }

            NotificationsTabSelected = true;
            NotificationsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/notifications_blue.png", UriKind.Relative));
            NotificationsTabTextBlockForeground = Application.Current.Resources["Blue"] as Brush;
        }

        public bool CanExecute_NotificationsTabCommand()
        {
            return true;
        }

        public MyICommand ShowUserPopUpCommand { get; set; }

        public void Executed_ShowUserPopUpCommand()
        {
            UserPopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_ShowUserPopUpCommand()
        {
            return true;
        }

        public MyICommand HideUserPopUpCommand { get; set; }

        public void Executed_HideUserPopUpCommand()
        {
            UserPopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_HideUserPopUpCommand()
        {
            return true;
        }

        public MyICommand ShowTutorialPopUpCommand { get; set; }

        public void Executed_ShowTutorialPopUpCommand()
        {
            UserPopUpVisibility = Visibility.Collapsed;
            TutorialPopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_ShowTutorialPopUpCommand()
        {
            return true;
        }

        public MyICommand HideTutorialPopUpCommand { get; set; }

        public void Executed_HideTutorialPopUpCommand()
        {
            PauseMedia();
            TutorialPopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_HideTutorialPopUpCommand()
        {
            return true;
        }

        public MyICommand<string> ChooseTutorialCommand { get; set; }

        public void Executed_ChooseTutorialCommand(string videoName)
        {
            MediaElementSource = _videos[videoName];
        }

        public bool CanExecute_ChooseTutorialCommand(string videoName)
        {
            return true;
        }

        public MyICommand PlayPauseCommand { get; set; }

        public void Executed_PlayPauseCommand()
        {
            if (_isMediaPlaying)
                PauseMedia();
            else
                PlayMedia();
        }

        public bool CanExecute_PlayPauseCommand()
        {
            return true;
        }

        public MyICommand ShowFeedbackPopUpCommand { get; set; }

        public void Executed_ShowFeedbackPopUpCommand()
        {
            UserPopUpVisibility = Visibility.Collapsed;
            FeedbackPopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_ShowFeedbackPopUpCommand()
        {
            return true;
        }

        public MyICommand HideFeedbackPopUpCommand { get; set; }

        public void Executed_HideFeedbackPopUpCommand()
        {
            FeedbackPopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_HideFeedbackPopUpCommand()
        {
            return true;
        }

        public MyICommand SendFeedbackCommand { get; set; }

        public void Executed_SendFeedbackCommand()
        {
            Feedback feedback = new Feedback();

            if (SelectedFeedbackIndex == 0)
                feedback.Type = FeedbackType.FAULT;
            else if (SelectedFeedbackIndex == 1)
                feedback.Type = FeedbackType.IMPROVEMENT;
            else
                feedback.Type = FeedbackType.QUESTION;

            feedback.Text = FeedbackText;
            feedback.SenderUsername = App.currentUser;

            _feedbackService.SendFeedback(feedback);

            MessageText = "Feedback sent successfully.";
            MessagePopUpVisibility = Visibility.Visible;
        }

        public bool CanExecute_SendFeedbackCommand()
        {
            return true;
        }

        public MyICommand CloseMessagePopUpCommand { get; set; }

        public void Executed_CloseMessagePopUpCommand()
        {
            MessagePopUpVisibility = Visibility.Collapsed;

            if (FeedbackPopUpVisibility == Visibility.Visible)
                FeedbackPopUpVisibility = Visibility.Collapsed;
        }

        public bool CanExecute_CloseMessagePopUpCommand()
        {
            return true;
        }

        public DoctorViewModel(MediaElement mediaElement)
        {
            Doctor doctor = (new DoctorService()).GetDoctor(App.currentUser);
            UserText = doctor.Name + " " + doctor.Surname;

            _mediaElement = mediaElement;
            _mediaElement.Loaded += _mediaElement_Loaded;
            _feedbackService = new FeedbackService();

            InitializeCommands();

            ScheduleTabSelected = true;
            ScheduleTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/schedule_blue.png", UriKind.Relative));
            ScheduleTabTextBlockForeground = Application.Current.Resources["Blue"] as Brush;

            PatientsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/patients_light_gray.png", UriKind.Relative));
            PatientsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            MedicinesTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/medicines_light_gray.png", UriKind.Relative));
            MedicinesTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;
            NotificationsTabImageSource = new BitmapImage(new Uri("../../../Images/Doctor/notifications_light_gray.png", UriKind.Relative));
            NotificationsTabTextBlockForeground = Application.Current.Resources["LightGray"] as Brush;

            UserPopUpVisibility = Visibility.Collapsed;
            TutorialPopUpVisibility = Visibility.Collapsed;
            FeedbackPopUpVisibility = Visibility.Collapsed;
            MessagePopUpVisibility = Visibility.Collapsed;

            PlayPauseButtonText = "Play";
        }

        private void InitializeCommands()
        {
            ScheduleTabCommand = new MyICommand(Executed_ScheduleTabCommand, CanExecute_ScheduleTabCommand);
            PatientsTabCommand = new MyICommand(Executed_PatientsTabCommand, CanExecute_PatientsTabCommand);
            MedicinesTabCommand = new MyICommand(Executed_MedicinesTabCommand, CanExecute_MedicinesTabCommand);
            NotificationsTabCommand = new MyICommand(Executed_NotificationsTabCommand, CanExecute_NotificationsTabCommand);
            ShowUserPopUpCommand = new MyICommand(Executed_ShowUserPopUpCommand, CanExecute_ShowUserPopUpCommand);
            HideUserPopUpCommand = new MyICommand(Executed_HideUserPopUpCommand, CanExecute_HideUserPopUpCommand);
            ShowTutorialPopUpCommand = new MyICommand(Executed_ShowTutorialPopUpCommand, CanExecute_ShowTutorialPopUpCommand);
            HideTutorialPopUpCommand = new MyICommand(Executed_HideTutorialPopUpCommand, CanExecute_HideTutorialPopUpCommand);
            ChooseTutorialCommand = new MyICommand<string>(Executed_ChooseTutorialCommand, CanExecute_ChooseTutorialCommand);
            PlayPauseCommand = new MyICommand(Executed_PlayPauseCommand, CanExecute_PlayPauseCommand);
            ShowFeedbackPopUpCommand = new MyICommand(Executed_ShowFeedbackPopUpCommand, CanExecute_ShowFeedbackPopUpCommand);
            HideFeedbackPopUpCommand = new MyICommand(Executed_HideFeedbackPopUpCommand, CanExecute_HideFeedbackPopUpCommand);
            SendFeedbackCommand = new MyICommand(Executed_SendFeedbackCommand, CanExecute_SendFeedbackCommand);
            CloseMessagePopUpCommand = new MyICommand(Executed_CloseMessagePopUpCommand, CanExecute_CloseMessagePopUpCommand);
        }

        private void _mediaElement_Loaded(object sender, RoutedEventArgs e)
        {
            PlayMedia();
        }

        private void PlayMedia()
        {
            _mediaElement.Play();
            _isMediaPlaying = true;
            PlayPauseButtonText = "Pause";
        }

        private void PauseMedia()
        {
            _mediaElement.Pause();
            _isMediaPlaying = false;
            PlayPauseButtonText = "Play";
        }
    }
}
