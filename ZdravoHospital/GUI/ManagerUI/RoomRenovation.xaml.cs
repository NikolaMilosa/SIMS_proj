using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.Logics;

namespace ZdravoHospital.GUI.ManagerUI
{
    /// <summary>
    /// Interaction logic for RoomRenovation.xaml
    /// </summary>
    public partial class RoomRenovation : Window, INotifyPropertyChanged
    {
        //Fields :
        private ObservableCollection<Room> _rooms;
        private Room _selectedRoom;
        private ObservableCollection<RoomScheduleDTO> _roomScheduleDTO;
        private string _startTime;
        private string _endTime;
        private DateTime _startDate;
        private DateTime _endDate;

        //Logics:
        private RoomFunctions roomFunctions;

        public ObservableCollection<Room> Rooms
        {
            get { return _rooms; }
            set
            {
                _rooms = value;
                OnPropertyChanged("Rooms");
            }
        }

        public Room SelectedRoom
        {
            get { return _selectedRoom; }
            set
            {
                _selectedRoom = value;
                RoomSchedule = roomFunctions.GetRoomSchedule(_selectedRoom);

                OnPropertyChanged("RoomSchedule");
                OnPropertyChanged("SelectedRoom");
            }
        }

        public ObservableCollection<RoomScheduleDTO> RoomSchedule
        {
            get { return _roomScheduleDTO; }
            set
            {
                _roomScheduleDTO = value;
                OnPropertyChanged("RoomReservations");
            }
        }

        public string StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = value;
                OnPropertyChanged("StartTime");
            }
        }

        public string EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = value;
                OnPropertyChanged("EndTime");
            }
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged("StartDate");
                EndDate = value;
                StartTime = "";
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                _endDate = value;
                OnPropertyChanged("EndDate");
                EndTime = "";
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

        public RoomRenovation()
        {
            InitializeComponent();
            this.DataContext = this;
            roomFunctions = new RoomFunctions();
            Rooms = new ObservableCollection<Room>(Model.Resources.rooms.Values);
            StartDate = DateTime.Today;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void ConfirmButton_Click(object sender, RoutedEventArgs e)
        {
            DateTime startTime = StartDate.Add(TimeSpan.ParseExact(StartTime, "c", null));
            DateTime endTime = EndDate.Add(TimeSpan.ParseExact(EndTime, "c", null));

            RoomSchedule roomSchedule = new RoomSchedule() { StartTime = startTime, EndTime = endTime, RoomId = SelectedRoom.Id };

            RoomScheduleFunctions roomScheduleFunctions = new RoomScheduleFunctions();
            roomScheduleFunctions.CreateAndScheduleRenovationStart(roomSchedule);

            this.Close();
        }

        private void FirstPicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!FirstPicker.IsDropDownOpen)
            {
                if (e.Key == Key.Enter)
                {
                    FirstPicker.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Tab) { }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    StartDate = (DateTime)FirstPicker.SelectedDate;
                    FirstPicker.IsDropDownOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Left) { }
                else if (e.Key == Key.Right) { }
                else if (e.Key == Key.Up) { }
                else if (e.Key == Key.Down) { }
                else if (e.Key == Key.Tab)
                {
                    FirstPicker.IsDropDownOpen = false;
                    e.Handled = true;
                    CancelButton.Focus();
                }
                else
                {
                    e.Handled = true;
                }
            }
        }

        private void SecondPicker_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (!SecondPicker.IsDropDownOpen)
            {
                if (e.Key == Key.Enter)
                {
                    SecondPicker.IsDropDownOpen = true;
                    e.Handled = true;
                }
                else if (e.Key == Key.Tab) { }
                else
                {
                    e.Handled = true;
                }
            }
            else
            {
                if (e.Key == Key.Enter)
                {
                    EndDate = (DateTime)SecondPicker.SelectedDate;
                    SecondPicker.IsDropDownOpen = false;
                    e.Handled = true;
                }
                else if (e.Key == Key.Left) { }
                else if (e.Key == Key.Right) { }
                else if (e.Key == Key.Up) { }
                else if (e.Key == Key.Down) { }
                else if (e.Key == Key.Tab)
                {
                    SecondPicker.IsDropDownOpen = false;
                    e.Handled = true;
                    CancelButton.Focus();
                }
                else
                {
                    e.Handled = true;
                }
            }
        }
    }
}
