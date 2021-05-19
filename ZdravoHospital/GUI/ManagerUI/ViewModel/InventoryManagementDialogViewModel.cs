using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Windows;
using Model;
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

        private ObservableCollection<Room> _senderRooms;
        private ObservableCollection<Room> _receiverRooms;

        private ObservableCollection<InventoryDTO> _senderRoomInventory;
        private ObservableCollection<InventoryDTO> _receiverRoomInventory;

        private InventoryDTO _selectedInventory;

        private Window _dialog;

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
            //SenderRooms = new ObservableCollection<Room>(Resources.rooms.Values);
        }

        #region Private functions

        private ObservableCollection<InventoryDTO> ConfigureTable(Room room)
        {
            var result = new ObservableCollection<InventoryDTO>();

            foreach (var roomInventory in Resources.roomInventory)
            {
                if (roomInventory.RoomId == room.Id)
                {
                    result.Add(new InventoryDTO(Resources.inventory[roomInventory.InventoryId].Name, roomInventory.Quantity,
                        roomInventory.InventoryId, Resources.inventory[roomInventory.InventoryId].InventoryType));
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
            SenderRoom = SenderRoom;
            ReceiverRoom = ReceiverRoom;
        }

        #endregion
    }
}
