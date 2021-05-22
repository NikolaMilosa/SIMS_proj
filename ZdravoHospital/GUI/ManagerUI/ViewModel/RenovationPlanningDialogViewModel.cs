using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.Logics;

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

        private RoomScheduleFunctions _roomScheduleFunctions;

        private RoomRepository _roomRepository;

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
                RoomSchedule = _roomScheduleFunctions.GetRoomSchedule(_selectedRoom);
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

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public RenovationPlanningDialogViewModel()
        {
            _roomScheduleFunctions = new RoomScheduleFunctions();
            _roomRepository = new RoomRepository();
            Rooms = new ObservableCollection<Room>(_roomRepository.GetValues());
            StartDate = DateTime.Today;

            ConfirmCommand = new MyICommand(OnConfirm);
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

            _roomScheduleFunctions.CreateAndScheduleRenovationStart(roomSchedule);
        }

        #endregion
    }
}
