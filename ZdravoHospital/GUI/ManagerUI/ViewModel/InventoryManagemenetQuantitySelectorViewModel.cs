using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.Commands;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.Logics;

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

        private TransferRequestsFunctions _transferRequestsFunctions;

        private Room _senderRoom;
        private Room _receiverRoom;
        private InventoryDTO _processedItem;

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

        public InventoryManagemenetQuantitySelectorViewModel(Room sender, Room receiver, InventoryDTO processed)
        {
            _transferRequestsFunctions = new TransferRequestsFunctions();

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

            foreach (var transferRequest in Resources.transferRequests)
            {
                if (transferRequest.SenderRoom == SenderRoom.Id &&
                    transferRequest.InventoryId.Equals(_processedItem.Id))
                    MaxInventory -= transferRequest.Quantity;
            }
        }

        private void SetDefinitionText()
        {
            DefinitionText = "Out of '" + MaxInventory + "' possible how many '" + Model.Resources.inventory[_processedItem.Id].Name
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

            _transferRequestsFunctions.CreateAndStartTransfer(newRequest);
        }

        private void MoveDynamic()
        {
            _transferRequestsFunctions.ExecuteRequest(new TransferRequest(SenderRoom.Id, ReceiverRoom.Id, _processedItem.Id, 
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
