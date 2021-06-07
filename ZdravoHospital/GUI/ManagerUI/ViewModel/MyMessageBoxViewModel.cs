using System;
using System.Collections.Generic;
using System.Text;
using ZdravoHospital.GUI.ManagerUI.View;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class MyMessageBoxViewModel : ViewModel
    {
        #region Fields

        private string _displayText;
        private MyMessageBox _dialog;

        #endregion

        #region Properties

        public string DisplayText
        {
            get => _displayText;
            set
            {
                _displayText = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public MyMessageBoxViewModel(string text)
        {
            DisplayText = text;
            _dialog = new MyMessageBox(this);
            _dialog.ShowDialog();
        }
    }
}
