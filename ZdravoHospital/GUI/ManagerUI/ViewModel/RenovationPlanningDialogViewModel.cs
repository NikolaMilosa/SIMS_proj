using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Model;
using Repository.PeriodPersistance;
using Repository.RoomPersistance;
using Repository.RoomSchedulePersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.View;
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

        private InjectorDTO _injector;

        private RoomScheduleService _roomScheduleService;
        private IRoomRepository _roomRepository;
        private IRoomScheduleRepository _roomScheduleRepository;
        private IPeriodRepository _periodRepository;

        private bool _isDropDownOpenCombo;
        private int _selectedRoomIndex;

        private bool _isDropDownOpenStartPicker;
        private bool _isDropDownOpenEndPicker;

        private Room _splitCreatedRoom;
        private string _roomButtonContent;

        private string _labelText;
        private MyICommand _splitCommand;

        private ObservableCollection<Room> _mergeRooms;
        private Room _mergeSelectedRoom;
        private int _mergeSelectedRoomIndex;
        private bool _mergeIsDropDownOpenCombo;
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

                FillMerged();

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
                EndTime = "";
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

        public Room SplitCreatedRoom
        {
            get => _splitCreatedRoom;
            set
            {
                _splitCreatedRoom = value;
                OnPropertyChanged();
            }
        }

        public string RoomButtonContent
        {
            get => _roomButtonContent;
            set
            {
                _roomButtonContent = value;
                OnPropertyChanged();
            }
        }

        public string LabelText
        {
            get => _labelText;
            set
            {
                _labelText = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<Room> MergeRooms
        {
            get => _mergeRooms;
            set
            {
                _mergeRooms = value;
                OnPropertyChanged();
            }
        }

        public Room MergeSelectedRoom
        {
            get => _mergeSelectedRoom;
            set
            {
                _mergeSelectedRoom = value;
                OnPropertyChanged();
            }
        }

        public int MergeSelectedRoomIndex
        {
            get => _mergeSelectedRoomIndex;
            set
            {
                _mergeSelectedRoomIndex = value;
                if (_mergeSelectedRoomIndex != 0)
                {
                    OnSplitButtonDestroy();
                }
                OnPropertyChanged();
            }
        }

        public bool MergeIsDropDownOpenCombo
        {
            get => _mergeIsDropDownOpenCombo;
            set
            {
                _mergeIsDropDownOpenCombo = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }
        public MyICommand<KeyEventArgs> ComboBoxCommand { get; set; }
        public MyICommand<KeyEventArgs> StartDateCommand { get; set; }
        public MyICommand<KeyEventArgs> EndDateCommand { get; set; }
        public MyICommand<KeyEventArgs> MergeComboBoxCommand { get; set; }

        public MyICommand SplitRoomCommand
        {
            get => _splitCommand;
            set
            {
                _splitCommand = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public RenovationPlanningDialogViewModel(InjectorDTO injector)
        {
            _injector = injector;
            
            LabelText = "";
            RoomButtonContent = "New room";

            _roomScheduleService = new RoomScheduleService(injector);
            _roomRepository = injector.RoomRepository;
            _roomScheduleRepository = injector.RoomScheduleRepository;
            _periodRepository = injector.PeriodRepository;

            Rooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            StartDate = DateTime.Today;

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<KeyEventArgs>(OnComboBox);
            StartDateCommand = new MyICommand<KeyEventArgs>(OnStartCommand);
            EndDateCommand = new MyICommand<KeyEventArgs>(OnEndCommand);
            SplitRoomCommand = new MyICommand(OnSplitButtonCreate);
            MergeComboBoxCommand = new MyICommand<KeyEventArgs>(OnMergeCombo);
        }

        #region Button functions

        private void OnSplitButtonCreate()
        {
            AddOrEditRoomDialog dialog = new AddOrEditRoomDialog(SplitCreatedRoom, _injector, true);
            ((AddOrEditRoomDialogViewModel)dialog.DataContext).RoomSplit += OnRoomSplit;
            dialog.Show();
        }

        private void OnSplitButtonDestroy()
        {
            LabelText = "";
            RoomButtonContent = "New room";
            SplitCreatedRoom = null;
            SplitRoomCommand = new MyICommand(OnSplitButtonCreate);
        }

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

            if (SplitCreatedRoom != null)
            {
                _roomRepository.Create(SplitCreatedRoom);

                var roomScheduleForSplitRoom = new RoomSchedule()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    RoomId = SplitCreatedRoom.Id,
                    ScheduleType = ReservationType.RENOVATION
                };

                _roomScheduleService.CreateAndScheduleRenovationStart(roomScheduleForSplitRoom);

                ManagerWindowViewModel.GetDashboard().OnRoomsChanged(this, EventArgs.Empty);
            }
            else if (MergeSelectedRoomIndex != 0)
            {
                var roomMergingSchedule = new RoomSchedule()
                {
                    StartTime = startTime,
                    EndTime = endTime,
                    RoomId = MergeSelectedRoom.Id,
                    MergingRoomId = SelectedRoom.Id,
                    ScheduleType = ReservationType.RENOVATION,
                    WillBeMerged = true
                };

                _roomScheduleService.CreateAndScheduleRenovationStart(roomMergingSchedule);
            }
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

        private void OnMergeCombo(KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                MergeIsDropDownOpenCombo = (MergeIsDropDownOpenCombo == false) ? true : false;
                e.Handled = true;
                if (!MergeIsDropDownOpenCombo && !MergeSelectedRoom.Name.Equals("No room"))
                {
                    OnSplitButtonDestroy();
                }
            }
            else if (e.Key == Key.Down)
            {
                if (MergeSelectedRoomIndex < MergeRooms.Count - 1 && MergeIsDropDownOpenCombo)
                {
                    MergeSelectedRoomIndex += 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (MergeSelectedRoomIndex > 0 && MergeIsDropDownOpenCombo)
                {
                    MergeSelectedRoomIndex -= 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                MergeIsDropDownOpenCombo = false;
            }
        }

        #endregion

        #region Event handler

        private void OnRoomSplit(object sender, RoomSplitEventArgs e)
        {
            SplitCreatedRoom = e.Room;
            LabelText = "Added! (" + SplitCreatedRoom.Id + ")";
            RoomButtonContent = "Clear room";
            SplitRoomCommand = new MyICommand(OnSplitButtonDestroy);
            MergeSelectedRoomIndex = 0;
        }

        #endregion

        #region Private functions

        private void FillMerged()
        {
            MergeRooms = new ObservableCollection<Room>(Rooms);
            MergeRooms.Remove(_selectedRoom);
            Room dummy = new Room()
            {
                Name = "No room",
                Id = 0
            };
            MergeRooms.Insert(0, dummy);

            var roomSchedule = _roomScheduleRepository.GetValues();
            foreach (var instance in roomSchedule)
            {
                if (instance.WillBeMerged)
                {
                    var temp = MergeRooms.ToList();
                    temp.RemoveAll(val => val.Id == instance.RoomId);
                    MergeRooms = new ObservableCollection<Room>(temp);
                }
            }

            MergeSelectedRoomIndex = 0;
        }

        #endregion
    }
}
