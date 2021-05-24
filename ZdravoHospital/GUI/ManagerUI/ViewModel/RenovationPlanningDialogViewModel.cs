using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows.Input;
using Model;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;
using RoomRepository = Repository.RoomPersistance.RoomRepository;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class RenovationPlanningDialogViewModel : ViewModel
    {
        #region Fields

        private ObservableCollection<Room> _rooms;
        private Room _selectedRoom;
        private ObservableCollection<RoomScheduleDTO> _roomScheduleDTO;
        private string _startTime;
        private string _endTime;
        private DateTime _startDate;
        private DateTime _endDate;

        private RoomScheduleService _roomScheduleService;

        private IRoomRepository _roomRepository;

        private bool _isDropDownOpenCombo;
        private int _selectedRoomIndex;

        private bool _isDropDownOpenStartPicker;
        private bool _isDropDownOpenEndPicker;

        #endregion

        #region Properties

        public ObservableCollection<Room> Rooms
        {
            get => _rooms;
            set
            {
                _rooms = value;
                OnPropertyChanged();
            }
        }

        public Room SelectedRoom
        {
            get => _selectedRoom;
            set
            {
                _selectedRoom = value;
                RoomSchedule = _roomScheduleService.GetRoomSchedule(_selectedRoom);
                OnPropertyChanged();
            }
        }

        public ObservableCollection<RoomScheduleDTO> RoomSchedule
        {
            get => _roomScheduleDTO;
            set
            {
                _roomScheduleDTO = value;
                OnPropertyChanged();
            }
        }

        public DateTime StartDate
        {
            get => _startDate;
            set
            {
                _startDate = value;
                OnPropertyChanged();
                if (EndDate <= StartDate)
                    EndDate = value;
                StartTime = "";
            }
        }

        public DateTime EndDate
        {
            get => _endDate;
            set
            {
                _endDate = value;
                OnPropertyChanged();
                EndTime = "";
            }
        }

        public string StartTime
        {
            get => _startTime;
            set
            {
                _startTime = value;
                OnPropertyChanged();
            }
        }

        public string EndTime
        {
            get => _endTime;
            set
            {
                _endTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpenCombo
        {
            get => _isDropDownOpenCombo;
            set
            {
                _isDropDownOpenCombo = value;
                OnPropertyChanged();
            }
        }

        public int SelectedRoomIndex
        {
            get => _selectedRoomIndex;
            set
            {
                _selectedRoomIndex = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpenStartPicker
        {
            get => _isDropDownOpenStartPicker;
            set
            {
                _isDropDownOpenStartPicker = value;
                OnPropertyChanged();
            }
        }

        public bool IsDropDownOpenEndPicker
        {
            get => _isDropDownOpenEndPicker;
            set
            {
                _isDropDownOpenEndPicker = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<KeyEventArgs> ComboBoxCommand { get; set; }
        public MyICommand<KeyEventArgs> StartDateCommand { get; set; }
        public MyICommand<KeyEventArgs> EndDateCommand { get; set; }

        #endregion

        public RenovationPlanningDialogViewModel(InjectorDTO injector)
        {
            _roomScheduleService = new RoomScheduleService(injector);
            _roomRepository = injector.RoomRepository;
            Rooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            StartDate = DateTime.Today;

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<KeyEventArgs>(OnComboBox);
            StartDateCommand = new MyICommand<KeyEventArgs>(OnStartCommand);
            EndDateCommand = new MyICommand<KeyEventArgs>(OnEndCommand);
        }

        #region Button functions

        private void OnConfirm()
        {
            var startTime = StartDate.Add(TimeSpan.ParseExact(StartTime, "c", null));
            var endTime = EndDate.Add(TimeSpan.ParseExact(EndTime, "c", null));

            var roomSchedule = new RoomSchedule()
            {
                StartTime = startTime,
                EndTime = endTime,
                RoomId = SelectedRoom.Id,
                ScheduleType = ReservationType.RENOVATION
            };

            _roomScheduleService.CreateAndScheduleRenovationStart(roomSchedule);
        }

        private void OnComboBox(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpenCombo = (IsDropDownOpenCombo == false) ? true : false;
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (SelectedRoomIndex < Rooms.Count - 1 && IsDropDownOpenCombo)
                {
                    SelectedRoomIndex += 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (SelectedRoomIndex > 0 && IsDropDownOpenCombo)
                {
                    SelectedRoomIndex -= 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpenCombo = false;
            }
        }

        private void OnStartCommand(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpenStartPicker = (IsDropDownOpenStartPicker == false) ? true : false;
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpenStartPicker = false;
            }
            else if (!IsDropDownOpenStartPicker)
            {
                e.Handled = true;
            }
        }

        private void OnEndCommand(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                IsDropDownOpenEndPicker = (IsDropDownOpenEndPicker == false) ? true : false;
            }
            else if (e.Key == Key.Tab)
            {
                IsDropDownOpenEndPicker = false;
            }
            else if (!IsDropDownOpenEndPicker)
            {
                e.Handled = true;
            }
        }
        
        #endregion
    }
}
