using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Model;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class FeedbackDialogViewModel : ViewModel
    {
        #region Fields

        private Feedback _newFeedback;

        private bool _isDropDownOpen;
        private int _selectedIndex;

        private FeedbackService _feedbackService;

        #endregion

        #region Properties

        public Feedback NewFeedback
        {
            get => _newFeedback;
            set
            {
                _newFeedback = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpen
        {
            get => _isDropDownOpen;
            set
            {
                _isDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public int SelectedIndex
        {
            get => _selectedIndex;
            set
            {
                _selectedIndex = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand<string> ComboBoxCommand { get; set; }
        public MyICommand SendCommand { get; set; }

        #endregion

        public FeedbackDialogViewModel(InjectorDTO injector, string currentUser)
        {
            _feedbackService = new FeedbackService(injector);

            NewFeedback = new Feedback();
            NewFeedback.SenderUsername = currentUser;
            ComboBoxCommand = new MyICommand<string>(OnComboBox);
            SendCommand = new MyICommand(OnSend);
        }

        #region Button functions

        private void OnComboBox(string key)
        {
            if (key.Equals("Enter"))
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Down"))
            {
                if (SelectedIndex < Enum.GetValues(typeof(FeedbackType)).Length - 1 && IsDropDownOpen)
                {
                    SelectedIndex += 1;
                }
            }
            else if (key.Equals("Up"))
            {
                if (SelectedIndex > 0 && IsDropDownOpen)
                {
                    SelectedIndex -= 1;
                }
            }
        }

        private void OnSend()
        {
            _feedbackService.SendFeedback(NewFeedback);
        }

        #endregion
    }
}
