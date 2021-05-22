using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;
using Repository.InventoryRepository;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using Repository.TransferRequestPersistance;
using ZdravoHospital.GUI.ManagerUI.Logics;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class TransferRequestService
    {
        #region Repos

        private IInventoryRepository _inventoryRepository;
        private IRoomRepository _roomRepository;
        private ITransferRequestRepository _transferRequestRepository;
        private IRoomInventoryRepository _roomInventoryRepository;

        #endregion

        #region Event things

        public delegate void TransferExecutedEventHandler(object sender, EventArgs e);

        public event TransferExecutedEventHandler TransferExecuted;

        protected virtual void OnTransferExecuted()
        {
            if (TransferExecuted != null)
            {
                TransferExecuted(this, EventArgs.Empty);
            }
        }

        #endregion

        public TransferRequestService()
        {
            TransferExecuted += ManagerWindowViewModel.GetDashboard().OnRefreshTransferDialog;

            _roomRepository = new RoomRepository();
            _inventoryRepository = new InventoryRepository();
            _transferRequestRepository = new TransferRequestRepository();
            _roomInventoryRepository = new RoomInventoryRepository();
        }

        public void RunOrExecute()
        {
            var values = _transferRequestRepository.GetValues();
            if (values.Count != 0)
            {
                List<TransferRequest> loaded = new List<TransferRequest>(values);
                foreach (TransferRequest tr in loaded)
                {
                    if (tr.TimeOfExecution < DateTime.Now)
                    {
                        ExecuteRequest(tr);
                    }
                    else
                    {
                        StartTransfer(tr);
                    }
                }
            }
        }

        private void StartTransfer(TransferRequest transferRequest)
        {
            Task t = new Task(() => transferRequest.DoWork());
            t.Start();
        }

        public void CreateAndStartTransfer(TransferRequest transferRequest)
        {
            _transferRequestRepository.Create(transferRequest);

            StartTransfer(transferRequest);

            /* Create a roomSchedule for this transfer */

            RoomScheduleService roomScheduleService = new RoomScheduleService();

            RoomSchedule roomScheduleSender = new RoomSchedule()
            {
                StartTime = transferRequest.TimeOfExecution,
                EndTime = transferRequest.TimeOfExecution.AddMinutes(2),
                RoomId = transferRequest.SenderRoom,
                ScheduleType = ReservationType.TRANSFER
            };
            roomScheduleService.CreateAndScheduleRenovationStart(roomScheduleSender);

            RoomSchedule roomScheduleReceiver = new RoomSchedule()
            {
                StartTime = transferRequest.TimeOfExecution.AddMinutes(2),
                EndTime = transferRequest.TimeOfExecution.AddMinutes(4),
                RoomId = transferRequest.RecipientRoom,
                ScheduleType = ReservationType.TRANSFER
            };
            roomScheduleService.CreateAndScheduleRenovationStart(roomScheduleReceiver);
        }

        public void ExecuteRequest(TransferRequest transferRequest)
        {
            var roomSender = _roomRepository.GetById(transferRequest.SenderRoom);
            var roomReceiver = _roomRepository.GetById(transferRequest.RecipientRoom);

            if (roomSender != null && roomReceiver != null && _inventoryRepository.GetById(transferRequest.InventoryId) != null)
            {
                /* Handle database transfer */
                var sender = _roomInventoryRepository.FindByBothIds(transferRequest.SenderRoom, transferRequest.InventoryId);
                var receiver = _roomInventoryRepository.FindByBothIds(transferRequest.RecipientRoom, transferRequest.InventoryId);

                if (sender.Quantity - transferRequest.Quantity == 0)
                {
                    _roomInventoryRepository.DeleteByEquality(sender);
                }
                else
                {
                    _roomInventoryRepository.SetNewQuantity(sender, sender.Quantity - transferRequest.Quantity);
                }

                if (receiver == null)
                {
                    _roomInventoryRepository.Create(new RoomInventory(transferRequest.InventoryId, transferRequest.RecipientRoom, transferRequest.Quantity));
                }
                else
                {
                    _roomInventoryRepository.SetNewQuantity(receiver, receiver.Quantity + transferRequest.Quantity);
                }
            }

            /* Serialize */
            _transferRequestRepository.DeleteByEquality(transferRequest);
            
            OnTransferExecuted();
        }

        public int GetScheduledInventoryForRoom(Inventory inventory, Room room)
        {
            int scheduledInventory = 0;

            _transferRequestRepository.GetValues().ForEach(tr =>
            {
                if (tr.SenderRoom == room.Id && tr.InventoryId.Equals(inventory.Id))
                {
                    scheduledInventory += tr.Quantity;
                }
            });

            return scheduledInventory;
        }
    }
}
