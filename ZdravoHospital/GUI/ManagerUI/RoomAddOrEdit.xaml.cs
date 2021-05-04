using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

using Model;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for RoomAddOrEdit.xaml
    /// </summary>
    public partial class RoomAddOrEdit : Window, INotifyPropertyChanged
    {
        //Fields:
        private int _id;
        private string _name;
        private RoomType _roomType;
        private Logics.RoomFunctions _roomFunctions;

        public int Id
        {
            get { return _id; }
            set
            {
                _id = value;
                OnPropertyChanged("Id");
            }
        }

        public string RoomName
        {
            get => _name;
            set
            {
                _name = value;
                OnPropertyChanged("RoomName");
            }
        }

        public RoomType RoomType
        {
            get { return _roomType; }
            set
            {
                _roomType = value;
                OnPropertyChanged("RoomType");
            }
        }

        bool _isAdder;

        public RoomAddOrEdit()
        {
            InitializeComponent();
            this.DataContext = this;

            this._roomFunctions = new Logics.RoomFunctions();

            _isAdder = true;
            this.Title = "Room adding dialog";
            TypeComboBox.SelectedIndex = 0;
            YesRadioButton.IsChecked = true;
        }

        public RoomAddOrEdit(Room r)
        {
            InitializeComponent();
            this.DataContext = this;

            this._roomFunctions = new Logics.RoomFunctions();

            IdTextBox.IsEnabled = false;

            _isAdder = false;
            this.Title = "Room editing dialog";
            Id = r.Id;
            RoomName = r.Name;
            RoomType = r.RoomType;

            if (r.Available == true)
                YesRadioButton.IsChecked = true;
            else
                NoRadioButton.IsChecked = true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            if (_isAdder)
            {
                var newRoom = new Room(RoomType, Id, RoomName, (YesRadioButton.IsChecked == true) ? true : false);
                _roomFunctions.AddRoom(newRoom);
            }
            else
            {
                var replaceRoom = new Room(RoomType, Id, RoomName, (YesRadioButton.IsChecked == true) ? true : false);
                _roomFunctions.EditRoom(replaceRoom);
            }

            this.Close();
        }

        private void YesRadioButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                YesRadioButton.IsChecked = true;
                e.Handled = true;
            }                
        }

        private void NoRadioButton_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                NoRadioButton.IsChecked = true;
                e.Handled = true;
            }
        }
    }
}
