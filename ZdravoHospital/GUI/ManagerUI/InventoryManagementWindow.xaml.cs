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
    /// Interaction logic for InventoryManagementWindow.xaml
    /// </summary>
    public partial class InventoryManagementWindow : Window, INotifyPropertyChanged
    {
        //Feilds:
        private List<Room> _firstRooms;
        private List<Room> _secondRooms;
        private Room _firstRoom;

        public List<Room> FirstRooms
        {
            get { return _firstRooms; }
            set
            {
                _firstRooms = value;
                OnPropertyChanged("FirstRooms");
            }
        }

        public List<Room> SecondRooms
        {
            get { return _secondRooms; }
            set
            {
                _secondRooms = value;
                OnPropertyChanged("SecondRooms");
            }
        }

        public Room FirstRoom
        {
            get { return _firstRoom; }
            set
            {
                _firstRoom = value;
                SecondRooms.Clear();
                SecondRooms.AddRange(FirstRooms);
                SecondRooms.Remove(FirstRoom);
                OnPropertyChanged("SecondRooms");
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string name)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }

        public InventoryManagementWindow()
        {
            InitializeComponent();
            FirstRooms = new List<Room>(Model.Resources.rooms.Values);
            MessageBox.Show(FirstRooms.Count.ToString());
            SecondRooms = new List<Room>();
        }

        
    }
}
