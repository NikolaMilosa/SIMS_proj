using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Input;
using Model;
using Model.Repository;
using Repository.InventoryPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.View;
using InventoryRepository = Repository.InventoryPersistance.InventoryRepository;
using RoomInventoryRepository = Repository.RoomInventoryPersistance.RoomInventoryRepository;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class InventoryManagementDialogViewModel : ViewModel
    {
        #region Fields

        private Room _senderRoom;
        private Room _receiverRoom;

        private int _senderIndex;
        private int _receiverIndex;
        private int _senderDataIndex;

        private ObservableCollection<Room> _senderRooms;
        private ObservableCollection<Room> _receiverRooms;

        private ObservableCollection<InventoryDTO> _senderRoomInventory;
        private ObservableCollection<InventoryDTO> _receiverRoomInventory;

        private InventoryDTO _selectedInventory;

        private Window _dialog;

        private IRoomRepository _roomRepository;

        private IRoomInventoryRepository _roomInventoryRepository;
        private IInventoryRepository _inventoryRepository;

        private InjectorDTO _injector;

        private bool _receiverIsDropDownOpen;
        private bool _senderIsDropDownOpen;

        private bool _focusLeftCombo;
        private bool _focusRightCombo;
        private bool _focusDataGrid;
        private bool _focusFinishButton;

        #endregion

        #region Properties

        public Room SenderRoom
        {
            get => _senderRoom;
            set
            {
                _senderRoom = value;

                if (_senderRoom != null)
                {
                    ReceiverRooms = new ObservableCollection<Room>(SenderRooms);
                    ReceiverRooms.Remove(SenderRoom);
                    SenderRoomInventory = ConfigureTable(SenderRoom);
                }
                else
                {
                    SenderRoomInventory = new ObservableCollection<InventoryDTO>();
                }

                OnPropertyChanged();
            }
        }

        public Room ReceiverRoom
        {
            get => _receiverRoom;
            set
            {
                _receiverRoom = value;

                if (_receiverRoom != null)
                {
                    ReceiverRoomInventory = ConfigureTable(ReceiverRoom);
                }
                else
                {
                    ReceiverRoomInventory = new ObservableCollection<InventoryDTO>();
                }

                OnPropertyChanged();
            }
        }
        
        public ObservableCollection<Room> SenderRooms
        {
            get => _senderRooms;
            set
            {
                _senderRooms = value;

                OnPropertyChanged();
            }
        }

        public ObservableCollection<Room> ReceiverRooms
        {
            get => _receiverRooms;
            set
            {
                _receiverRooms = value;
                OnPropertyChanged();
            }
        }

        public InventoryDTO SelectedInventory
        {
            get => _selectedInventory;
            set
            {
                _selectedInventory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<InventoryDTO> SenderRoomInventory
        {
            get => _senderRoomInventory;
            set
            {
                _senderRoomInventory = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<InventoryDTO> ReceiverRoomInventory
        {
            get => _receiverRoomInventory;
            set
            {
                _receiverRoomInventory = value;
                OnPropertyChanged();
            }
        }

        public bool SenderIsDropDownOpen
        {
            get => _senderIsDropDownOpen;
            set
            {
                _senderIsDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public int SenderIndex
        {
            get => _senderIndex;
            set
            {
                _senderIndex = value;
                OnPropertyChanged();
            }
        }

        public int ReceiverIndex
        {
            get => _receiverIndex;
            set
            {
                _receiverIndex = value;
                OnPropertyChanged();
            }
        }

        public bool ReceiverIsDropDownOpen
        {
            get => _receiverIsDropDownOpen;
            set
            {
                _receiverIsDropDownOpen = value;
                OnPropertyChanged();
            }
        }

        public int SenderDataIndex
        {
            get => _senderDataIndex;
            set
            {
                _senderDataIndex = value;
                OnPropertyChanged();
            }
        }

        public bool FocusLeftCombo
        {
            get => _focusLeftCombo;
            set
            {
                _focusLeftCombo = value;
                OnPropertyChanged();
            }
        }

        public bool FocusRightCombo
        {
            get => _focusRightCombo;
            set
            {
                _focusRightCombo = value;
                OnPropertyChanged();
            }
        }

        public bool FocusDataGrid
        {
            get => _focusDataGrid;
            set
            {
                _focusDataGrid = value;
                OnPropertyChanged();
            }
        }

        public bool FocusFinishButton
        {
            get => _focusFinishButton;
            set
            {
                _focusFinishButton = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand<string> SenderCombo { get; set; }
        public MyICommand<string> ReceiverCombo { get; set; }
        public MyICommand<KeyEventArgs> DataGridCommand { get; set; }

        #endregion

        public InventoryManagementDialogViewModel(InjectorDTO injector)
        {
            _roomRepository = injector.RoomRepository;
            _roomInventoryRepository = injector.RoomInventoryRepository;
            _inventoryRepository = injector.InventoryRepository;

            _injector = injector;

            SenderCombo = new MyICommand<string>(OnComboBoxSender);
            ReceiverCombo = new MyICommand<string>(OnComboBoxReceiver);
            DataGridCommand = new MyICommand<KeyEventArgs>(OnDataGridCommand);
        }

        #region Private functions

        private ObservableCollection<InventoryDTO> ConfigureTable(Room room)
        {
            var result = new ObservableCollection<InventoryDTO>();

            foreach (var roomInventory in _roomInventoryRepository.GetValues())
            {
                if (roomInventory.RoomId == room.Id)
                {
                    var inventory = _inventoryRepository.GetById(roomInventory.InventoryId);
                    result.Add(new InventoryDTO(inventory.Name, roomInventory.Quantity,
                        roomInventory.InventoryId, inventory.InventoryType));
                }
            }

            return result;
        }

        private void TurnOffFocus()
        {
            FocusDataGrid = false;
            FocusLeftCombo = false;
            FocusRightCombo = false;
            FocusFinishButton = false;
        }

        #endregion

        #region Complex Key handling

        public void HandleEnter()
        {
            _dialog = new InventoryManagementQuantitySelector(SenderRoom, ReceiverRoom, SelectedInventory, _injector);
            _dialog.ShowDialog();
        }

        #endregion

        #region Public functions

        public void OnShoudRefresh()
        {
            var tempRoomSender = SenderRoom;
            var tempRoomReceiver = ReceiverRoom;

            SenderRoom = null;
            ReceiverRoom = null;

            var rooms = _roomRepository.GetValues();
            SenderRooms = new ObservableCollection<Room>(rooms);

            if (tempRoomSender != null)
                SenderRoom = rooms.Find(r => r.Id == tempRoomSender.Id);
            if (tempRoomReceiver != null)
                ReceiverRoom = rooms.Find(r => r.Id == tempRoomReceiver.Id);
        }

        #endregion

        #region Button functions

        private void OnComboBoxSender(string key)
        {
            if (key.Equals("Enter"))
            {
                SenderIsDropDownOpen = (SenderIsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Up"))
            {
                if (SenderIsDropDownOpen && SenderIndex > 0)
                {
                    SenderIndex -= 1;
                }
            }
            else if (key.Equals("Down"))
            {
                if (SenderIsDropDownOpen && SenderIndex < SenderRooms.Count - 1)
                {
                    SenderIndex += 1;
                }
            }
            else if (key.Equals("Right"))
            {
                TurnOffFocus();
                FocusRightCombo = true;
            }
            else if (key.Equals("Tab"))
            {
                TurnOffFocus();
                FocusDataGrid = true;
            }
            else if (key.Equals("Esc"))
            {
                TurnOffFocus();
                FocusFinishButton = true;
            }
        }

        private void OnComboBoxReceiver(string key)
        {
            if (key.Equals("Enter"))
            {
                ReceiverIsDropDownOpen = (ReceiverIsDropDownOpen == false) ? true : false;
            }
            else if (key.Equals("Up"))
            {
                if (ReceiverIsDropDownOpen && ReceiverIndex> 0)
                {
                    ReceiverIndex -= 1;
                }
            }
            else if (key.Equals("Down"))
            {
                if (ReceiverIsDropDownOpen && ReceiverIndex < ReceiverRooms.Count - 1)
                {
                    ReceiverIndex += 1;
                }
            }
            else if (key.Equals("Left"))
            {
                TurnOffFocus();
                FocusLeftCombo = true;
            }
            else if (key.Equals("Tab"))
            {
                TurnOffFocus();
                FocusDataGrid = true;
            }
            else if (key.Equals("Esc"))
            {
                TurnOffFocus();
                FocusFinishButton = true;
            }
        }

        private void OnDataGridCommand(KeyEventArgs e)
        {
            if (e.Key == Key.Left)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                if (SenderDataIndex < SenderRoomInventory.Count - 1)
                {
                    SenderDataIndex += 1;
                }   

                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                if (SenderDataIndex > 0)
                {
                    SenderDataIndex -= 1;
                }

                e.Handled = true;
            }
            else if (e.Key == Key.Right)
            {
                e.Handled = true;
            }
            else if (e.Key == Key.Tab)
            {
                TurnOffFocus();
                FocusFinishButton = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Escape)
            {
                TurnOffFocus();
                FocusLeftCombo = true;
                e.Handled = true;
            }
            else if (e.Key == Key.Enter)
            {
                HandleEnter();
                e.Handled = true;
            }
        }
        
        #endregion
    }
}
