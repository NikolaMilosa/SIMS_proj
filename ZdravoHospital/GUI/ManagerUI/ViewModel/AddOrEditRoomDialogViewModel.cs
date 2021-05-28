using System;
using System.Collections.Generic;
using System.Text;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class AddOrEditRoomDialogViewModel : ViewModel
    {
        #region Fields

        private string _title;
        private Room _room;
        private bool _isAdder;
        private bool _available;
        private RoomService _roomService;

        private bool _isDropDownOpen;
        private int _selectedIndex;

        #endregion
        
        #region Properties

        public string Title
        {
            get => _title;
            set
            {
                _title = value;
                OnPropertyChanged();
            }
        }

        public Room Room
        {
            get => _room;
            set
            {
                _room = value;
                OnPropertyChanged();
                OnPropertyChanged("Available");
            }
        }

        public bool IsAdder
        {
            get => _isAdder;
            set
            {
                _isAdder = value;
                OnPropertyChanged();
            }
        }

        public string Available
        {
            get
            {
                if (_available == true)
                    return "YES";
                return "NO";
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
        
        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<string> ComboBoxCommand { get; set; }

        #endregion

        #region Event Things

        public delegate void RoomSplitEventHandler(object sender, RoomSplitEventArgs e);

        public event RoomSplitEventHandler RoomSplit;

        protected virtual void OnRoomSplit()
        {
            if (RoomSplit != null)
            {
                RoomSplit(this,  new RoomSplitEventArgs() {Room = Room});
            }
        }

        #endregion

        public AddOrEditRoomDialogViewModel(Room? room, InjectorDTO injector, bool roomSplitter)
        {
            if (room == null)
            {
                Room = new Room(RoomType.APPOINTMENT_ROOM, 0, "", true);
                _available = true;
                IsAdder = true;
                Title = "Room adding";
            }
            else
            {
                Room = new Room(room.RoomType, room.Id, room.Name, room.Available);
                _available = Room.Available;
                SelectedIndex = (int)Room.RoomType;
                IsAdder = false;
                Title = "Room editing";
            }

            _roomService = new RoomService(injector);
            
            if (!roomSplitter)
                ConfirmCommand = new MyICommand(OnConfirm);
            else
            {
                Room.Available = false;
                _available = false;
                ConfirmCommand = new MyICommand(OnConfirmSplit);
            }

            ComboBoxCommand = new MyICommand<string>(OnComboBox);
        }

        #region Button functions

        private void OnConfirm()
        {
            if (IsAdder)
            {
                _roomService.AddRoom(Room);
            }
            else
            {
                _roomService.EditRoom(Room);
            }
        }

        private void OnConfirmSplit()
        {
            OnRoomSplit();
        }

        private void OnComboBox(string key)
        {
            if (key.Equals("Enter"))
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Down"))
            {
                if (SelectedIndex < Enum.GetValues(typeof(RoomType)).Length - 1 && IsDropDownOpen)
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

        #endregion
    }

    public class RoomSplitEventArgs : EventArgs
    {
        public Room Room { get; set; }
    }
}
