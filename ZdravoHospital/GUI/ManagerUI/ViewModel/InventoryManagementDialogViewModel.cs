using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.Logics;
using ZdravoHospital.GUI.ManagerUI.View;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    public class InventoryManagementDialogViewModel : ViewModel
    {
        #region Fields

        private Room _senderRoom;
        private Room _receiverRoom;

        private int _senderIndex;
        private int _receiverIndex;

        private ObservableCollection<Room> _senderRooms;
        private ObservableCollection<Room> _receiverRooms;

        private ObservableCollection<InventoryDTO> _senderRoomInventory;
        private ObservableCollection<InventoryDTO> _receiverRoomInventory;

        private InventoryDTO _selectedInventory;

        private Window _dialog;

        private RoomRepository _roomRepository;

        private RoomInventoryRepository _roomInventoryRepository;
        private InventoryRepository _inventoryRepository;

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
        #endregion

        public InventoryManagementDialogViewModel()
        {
            _roomRepository = new RoomRepository();
            _roomInventoryRepository = new RoomInventoryRepository();
            _inventoryRepository = new InventoryRepository();
            SenderRooms = new ObservableCollection<Room>(_roomRepository.GetValues());
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

        #endregion

        #region Complex Key handling

        public void HandleEnter()
        {
            _dialog = new InventoryManagementQuantitySelector(SenderRoom, ReceiverRoom, SelectedInventory);
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
    }
}
