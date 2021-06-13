using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.PeriodPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;
using RoomRepository = Repository.RoomPersistance.RoomRepository;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class InventoryAdderSubtractorViewModel : ViewModel
    {
        #region Fields

        private string _selectedInventory;
        private List<Room> _rooms;
        private Room _selectedRoom;
        private string _roomInventoryQuantity;
        private int _enteredQuantity;
        private int _minInventory;

        private TransferRequestService _transferRequestsService;
        private InventoryService _inventoryService;

        private IRoomRepository _roomRepository;
        private IRoomInventoryRepository _roomInventoryRepository;
        private IPeriodRepository _periodRepository;

        private int _selectedIndex;
        private bool _isDropDownOpen;

        #endregion

        #region Properties

        public string SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged();
            }
        }

        public List<Room> Rooms
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
                OnPropertyChanged();

                if (_selectedRoom != null)
                {
                    MinInventory = _transferRequestsService.GetScheduledInventoryForRoom(PassedInventory, _selectedRoom);

                    var roomInventory = _roomInventoryRepository.FindByBothIds(_selectedRoom.Id, PassedInventory.Id);

                    var parts = SelectedInventory.Split("-");
                    if (parts[1].Trim().ToLower().Equals("bed"))
                    {
                        MinInventory += GetTakenBeds();
                    }

                    if (PassedInventory.InventoryType == InventoryType.DYNAMIC_INVENTORY && roomInventory != null)
                    {
                        RoomInventoryQuantity = roomInventory.Quantity.ToString();
                    }
                    else if (PassedInventory.InventoryType == InventoryType.STATIC_INVENTORY && roomInventory != null)
                    {
                        if (MinInventory != 0)
                            RoomInventoryQuantity = ((new StringBuilder()).Append(roomInventory.Quantity.ToString()).Append(" (")
                                .Append(MinInventory)
                                .Append(")")).ToString();
                        else
                            RoomInventoryQuantity = roomInventory.Quantity.ToString();
                    }
                    else
                    {
                        RoomInventoryQuantity = "0";
                    }

                    EnteredQuantity = 0;
                }
            }
        }

        public string RoomInventoryQuantity
        {
            get => _roomInventoryQuantity;
            set
            {
                _roomInventoryQuantity = value;
                OnPropertyChanged();
            }
        }

        public int EnteredQuantity
        {
            get => _enteredQuantity;
            set
            {
                _enteredQuantity = value;
                OnPropertyChanged();
            }
        }

        public int MinInventory
        {
            get => _minInventory;
            set
            {
                _minInventory = value;
                OnPropertyChanged();
            }
        }

        public Inventory PassedInventory { get; set; }

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

        public InventoryAdderSubtractorViewModel(Inventory inventory, InjectorDTO injector)
        {
            PassedInventory = inventory;

            _transferRequestsService = new TransferRequestService(injector);
            _inventoryService = new InventoryService(injector, ManagerWindowViewModel.GetDashboard());

            _roomRepository = injector.RoomRepository;
            _roomInventoryRepository = injector.RoomInventoryRepository;
            _periodRepository = injector.PeriodRepository;

            Rooms = _roomRepository.GetByType(RoomType.STORAGE_ROOM);
            if (Rooms.Count == 0)
            {
                Rooms = _roomRepository.GetValues();
            }
            SelectedInventory = ((new StringBuilder()).Append(PassedInventory.Id).Append(" - ").Append(PassedInventory.Name)).ToString();

            ConfirmCommand = new MyICommand(OnConfirm);
            ComboBoxCommand = new MyICommand<string>(OnComboBox);
        }

        #region Button functions

        private void OnConfirm()
        {
            _inventoryService.EditInventoryAmount(PassedInventory, EnteredQuantity, SelectedRoom);
        }

        private void OnComboBox(string key)
        {
            if (key.Equals("Enter"))
            {
                IsDropDownOpen = (IsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Up"))
            {
                if (IsDropDownOpen && SelectedIndex > 0)
                {
                    SelectedIndex -= 1;
                }
            }
            else if (key.Equals("Down"))
            {
                if (IsDropDownOpen && SelectedIndex < Rooms.Count - 1)
                {
                    SelectedIndex += 1;
                }
            }
        }

        #endregion

        #region Private functions

        private int GetTakenBeds()
        {
            int ret = 0;

            var periods = _periodRepository.GetValues();

            foreach (var period in periods)
            {
                if (period.Treatment == null)
                {
                    continue;
                }

                if (period.Treatment.RoomId == SelectedRoom.Id &&
                    period.Treatment.StartDate > DateTime.Now)
                {
                    ret++;
                }
            }

            return ret;
        }

        #endregion
    }
}
