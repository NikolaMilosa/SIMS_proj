using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.Logics;

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

        private TransferRequestsFunctions _transferRequestsFunctions;
        private RoomInventoryFunctions _roomInventoryFunctions;
        private InventoryFunctions _inventoryFunctions;

        private RoomRepository _roomRepository;

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
                    MinInventory = _transferRequestsFunctions.GetScheduledInventoryForRoom(PassedInventory, _selectedRoom);

                    var roomInventory = _roomInventoryFunctions.FindRoomInventoryByRoomAndInventory(_selectedRoom.Id, PassedInventory.Id);

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

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public InventoryAdderSubtractorViewModel(Inventory inventory)
        {
            PassedInventory = inventory;

            _transferRequestsFunctions = new TransferRequestsFunctions();
            _roomInventoryFunctions = new RoomInventoryFunctions();
            _inventoryFunctions = new InventoryFunctions();

            _roomRepository = new RoomRepository();

            Rooms = new List<Room>(_roomRepository.GetValues());
            SelectedInventory = ((new StringBuilder()).Append(PassedInventory.Id).Append(" - ").Append(PassedInventory.Name)).ToString();

            ConfirmCommand = new MyICommand(OnConfirm);
        }

        #region Button functions

        private void OnConfirm()
        {
            _inventoryFunctions.EditInventoryAmount(PassedInventory, EnteredQuantity, SelectedRoom);
        }

        #endregion
    }
}
