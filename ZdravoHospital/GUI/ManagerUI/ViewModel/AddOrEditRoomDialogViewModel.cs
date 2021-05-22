using System;
using System.Collections.Generic;
using System.Text;
using Model;
using ZdravoHospital.GUI.ManagerUI.Commands;
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

        #endregion
        
        #region Commands
        
        public MyICommand ConfirmCommand { get; set; }
        
        #endregion
        
        public AddOrEditRoomDialogViewModel(Room? room)
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
                IsAdder = false;
                Title = "Room editing";
            }

            _roomService = new RoomService();

            ConfirmCommand = new MyICommand(OnConfirm);
        }

        //Confirm command function 
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
    }
}
