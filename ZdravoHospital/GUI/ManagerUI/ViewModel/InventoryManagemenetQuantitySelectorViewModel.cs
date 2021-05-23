using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.InventoryPersistance;
using Repository.TransferRequestPersistance;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.Services.Manager;
using InventoryRepository = Repository.InventoryPersistance.InventoryRepository;
using TransferRequestRepository = Repository.TransferRequestPersistance.TransferRequestRepository;

namespace ZdravoHospital.GUI.ManagerUI.ViewModel
{
    class InventoryManagemenetQuantitySelectorViewModel : ViewModel
    {
        #region Fields

        private int _maxInventory;
        private string _definitionText;
        private int _enteredQuantity;
        private DateTime _chosenDate;
        private string _inputTime;
        private bool _isStatic;

        private TransferRequestService _transferRequestsService;

        private Room _senderRoom;
        private Room _receiverRoom;
        private InventoryDTO _processedItem;

        private IInventoryRepository _inventoryRepository;
        private ITransferRequestRepository _transferRequestRepository;

        #endregion

        #region Properties

        public int MaxInventory
        {
            get => _maxInventory;
            set
            {
                _maxInventory = value;
                OnPropertyChanged();
            }
        }

        public string DefinitionText
        {
            get => _definitionText;
            set
            {
                _definitionText = value;
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

        public DateTime ChosenDate
        {
            get => _chosenDate;
            set
            {
                _chosenDate = value;
                OnPropertyChanged();
            }
        }

        public string InputTime
        {
            get => _inputTime;
            set
            {
                _inputTime = value;
                OnPropertyChanged();
            }
        }

        public bool IsStatic
        {
            get => _isStatic;
            set
            {
                _isStatic = value;
                OnPropertyChanged();
            }
        }

        public Room SenderRoom
        {
            get => _senderRoom;
            set
            {
                _senderRoom = value;
                OnPropertyChanged();
            }
        }
        public Room ReceiverRoom
        {
            get => _receiverRoom;
            set
            {
                _receiverRoom = value;
                OnPropertyChanged();
            }
        }

        #endregion

        #region Commands

        public MyICommand ConfirmCommand { get; set; }

        #endregion

        public InventoryManagemenetQuantitySelectorViewModel(Room sender, Room receiver, InventoryDTO processed, InjectorDTO injector)
        {
            _transferRequestsService = new TransferRequestService(injector);

            _inventoryRepository = injector.InventoryRepository;
            _transferRequestRepository = injector.TransferRepository;

            SenderRoom = sender;
            ReceiverRoom = receiver;
            IsStatic = (processed.InventoryType == InventoryType.STATIC_INVENTORY);
            ChosenDate = DateTime.Today;
            _processedItem = processed;

            ConfigureMaxQuantity();
            SetDefinitionText();
            CleanIfDynamic();

            ConfirmCommand = new MyICommand(OnConfirmCommand);
        }

        #region Private functions

        private void ConfigureMaxQuantity()
        {
            MaxInventory = _processedItem.Quantity;

            foreach (var transferRequest in _transferRequestRepository.GetValues())
            {
                if (transferRequest.SenderRoom == SenderRoom.Id &&
                    transferRequest.InventoryId.Equals(_processedItem.Id))
                    MaxInventory -= transferRequest.Quantity;
            }
        }

        private void SetDefinitionText()
        {
            var inventory = _inventoryRepository.GetById(_processedItem.Id);

            DefinitionText = "Out of '" + MaxInventory + "' possible how many '" + inventory.Name
                             + "' would you like to transfer?";

            if (MaxInventory != _processedItem.Quantity)
            {
                DefinitionText += "\nThis room has '" + (_processedItem.Quantity - MaxInventory) + "' units scheduled for transfer.";
            }

        }

        private void CleanIfDynamic()
        {
            if (!IsStatic)
            {
                ChosenDate = DateTime.Today.AddDays(1);
                InputTime = "0:0";
            }
        }

        private void OnConfirmCommand()
        {
            if (IsStatic)
            {
                MoveStatic();
            }
            else
            {
                MoveDynamic();
            }
        }

        private void MoveStatic()
        {
            TimeSpan enteredTime = TimeSpan.ParseExact(InputTime, "c", null);
            ChosenDate = ChosenDate.Add(enteredTime);

            TransferRequest newRequest = new TransferRequest(SenderRoom.Id, ReceiverRoom.Id, _processedItem.Id,
                EnteredQuantity, ChosenDate);

            _transferRequestsService.CreateAndStartTransfer(newRequest);
        }

        private void MoveDynamic()
        {
            _transferRequestsService.ExecuteRequest(new TransferRequest(SenderRoom.Id, ReceiverRoom.Id, _processedItem.Id, 
                                                    EnteredQuantity, DateTime.Now));
        }

        #endregion

        #region Public functions

        public void SetChosenDate(DateTime passedDate)
        {
            ChosenDate = passedDate;
        }

        #endregion
    }
}
