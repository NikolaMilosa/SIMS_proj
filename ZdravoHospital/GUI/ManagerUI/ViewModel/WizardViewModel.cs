using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using ZdravoHospital.GUI.DoctorUI.Commands;
using ZdravoHospital.GUI.ManagerUI.View;
using ZdravoHospital.GUI.ManagerUI.View.WizardUserControls;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class WizardViewModel : ViewModel
    {
        #region Fields

        private Window _window;
        private string _activeUser;

        private bool _shouldDisplayNext;
        private bool _shouldDisplayPrevious;

        private UserControl _currentControl;

        #endregion

        #region Properties

        private List<UserControl> UserControls { get; set; }

        private int CurrentIndex { get; set; }

        public UserControl CurrentControl
        {
            get => _currentControl;
            set
            {
                _currentControl = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldDisplayNext
        {
            get => _shouldDisplayNext;
            set
            {
                _shouldDisplayNext = value;
                OnPropertyChanged();
            }
        }

        public bool ShouldDisplayPrevious
        {
            get => _shouldDisplayPrevious;
            set
            {
                _shouldDisplayPrevious = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand<object> SkipCommand { get; set; }
        public MyICommand NextCommand { get; set; }
        public MyICommand PreviousCommand { get; set; }

        #endregion

        public WizardViewModel(string activeUser)
        {
            _activeUser = activeUser;
            SkipCommand = new MyICommand<object>(OnSkipCommand);
            NextCommand = new MyICommand(OnNext);
            PreviousCommand = new MyICommand(OnPrevious);

            InitializeUserControls();
        }

        #region Button functions

        private void OnSkipCommand(object window)
        {
            _window = new ManagerWindow(_activeUser);
            _window.Show();

            (window as Window).Close();
        }

        private void OnNext()
        {
            CurrentControl = UserControls[++CurrentIndex];
            ResolveVisibility();
        }

        private void OnPrevious()
        {
            CurrentControl = UserControls[--CurrentIndex];
            ResolveVisibility();
        }

        #endregion

        #region Private functions

        private void InitializeUserControls()
        {
            UserControls = new List<UserControl>();

            UserControls.Add(new WizardIntro());
            UserControls.Add(new WizardDashboard1());
            UserControls.Add(new WizardDashboard2());
            UserControls.Add(new WizardDashboard3());
            UserControls.Add(new WizardDashboard4());
            UserControls.Add(new WizardAddEditRoom1());
            UserControls.Add(new WizardAddEditRoom2());
            UserControls.Add(new WizardAddEditRoom3());
            UserControls.Add(new WizardAddEditRoom4());
            UserControls.Add(new WizardAddEditRoom5());
            UserControls.Add(new WizardDeleteRoom());
            UserControls.Add(new WizardManageInventory1());
            UserControls.Add(new WizardManageInventory2());
            UserControls.Add(new WizardManageInventory3());
            UserControls.Add(new WizardManageInventory4());
            UserControls.Add(new WizardManageInventory5());
            UserControls.Add(new WizardPlanningRenovation1());
            UserControls.Add(new WizardPlanningRenovation2());
            UserControls.Add(new WizardPlanningRenovation3());
            UserControls.Add(new WizardStaffReport1());


            CurrentControl = UserControls[CurrentIndex];
            ResolveVisibility();
        }

        private void ResolveVisibility()
        {
            if (CurrentIndex == 0)
            {
                ShouldDisplayNext = true;
                ShouldDisplayPrevious = false;
            }
            else if (CurrentIndex == UserControls.Count - 1)
            {
                ShouldDisplayNext = false;
                ShouldDisplayPrevious = true;
            }
            else
            {
                ShouldDisplayNext = true;
                ShouldDisplayPrevious = true;
            }
        }

        #endregion
    }
}
