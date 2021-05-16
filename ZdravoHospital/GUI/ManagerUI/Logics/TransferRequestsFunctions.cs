using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class TransferRequestsFunctions
    {
        private static Mutex transferRequestMutex;

        public static Mutex GetTransferRequestMutex()
        {
            if (transferRequestMutex == null)
                transferRequestMutex = new Mutex();

            return transferRequestMutex;
        }

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

        public TransferRequestsFunctions()
        {
            TransferExecuted += ViewModel.ManagerWindowViewModel.GetDashboard().OnRefreshRenovationDialog;
        }

        public void RunOrExecute()
        {
            if (Model.Resources.transferRequests.Count != 0)
            {
                List<TransferRequest> loaded = new List<TransferRequest>(Model.Resources.transferRequests);
                foreach(TransferRequest tr in loaded)
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

        public void StartTransfer(TransferRequest transferRequest)
        {
            Task t = new Task(() => transferRequest.DoWork());
            t.Start();
        }

        public void CreateAndStartTransfer(TransferRequest transferRequest)
        {
            GetTransferRequestMutex().WaitOne();

            Model.Resources.transferRequests.Add(transferRequest);
            Model.Resources.SaveTransferRequests();

            GetTransferRequestMutex().ReleaseMutex();

            StartTransfer(transferRequest);

            /* Create a roomSchedule for this transfer */
            
            RoomScheduleFunctions roomScheduleFunctions = new RoomScheduleFunctions();
            
            RoomSchedule roomScheduleSender = new RoomSchedule() { StartTime = transferRequest.TimeOfExecution, EndTime = transferRequest.TimeOfExecution.AddMinutes(2), RoomId = transferRequest.SenderRoom, ScheduleType = ReservationType.TRANSFER };
            roomScheduleFunctions.CreateAndScheduleRenovationStart(roomScheduleSender);

            RoomSchedule roomScheduleReceiver = new RoomSchedule() { StartTime = transferRequest.TimeOfExecution.AddMinutes(2), EndTime = transferRequest.TimeOfExecution.AddMinutes(4), RoomId = transferRequest.RecipientRoom, ScheduleType = ReservationType.TRANSFER };
            roomScheduleFunctions.CreateAndScheduleRenovationStart(roomScheduleReceiver);
        }

        public void ExecuteRequest(TransferRequest transferRequest)
        {
            GetTransferRequestMutex().WaitOne();

            if (Model.Resources.rooms.ContainsKey(transferRequest.SenderRoom) && Model.Resources.rooms.ContainsKey(transferRequest.RecipientRoom) && Model.Resources.inventory.ContainsKey(transferRequest.InventoryId))
            {
                var roomInventoryService = new RoomInventoryFunctions();
                /* Handle database transfer */
                var sender = roomInventoryService.FindRoomInventoryByRoomAndInventory(transferRequest.SenderRoom, transferRequest.InventoryId);
                var receiver = roomInventoryService.FindRoomInventoryByRoomAndInventory(transferRequest.RecipientRoom, transferRequest.InventoryId);
                
                if (sender.Quantity - transferRequest.Quantity == 0)
                {
                    roomInventoryService.DeleteByReference(sender);
                }
                else
                {
                    roomInventoryService.SetNewQuantity(sender, sender.Quantity - transferRequest.Quantity);
                }

                if (receiver == null)
                {
                    roomInventoryService.AddNewReference(new RoomInventory(transferRequest.InventoryId, transferRequest.RecipientRoom, transferRequest.Quantity));
                }
                else
                {
                    roomInventoryService.SetNewQuantity(receiver, receiver.Quantity + transferRequest.Quantity);
                }
            }

            /* Serialize */
            if (Model.Resources.transferRequests.Remove(transferRequest))
                Model.Resources.SaveTransferRequests();

            GetTransferRequestMutex().ReleaseMutex();
            OnTransferExecuted();
        }

        public int GetScheduledInventoryForRoom(Inventory inventory, Room room)
        {
            int scheduledInventory = 0;

            Model.Resources.transferRequests.ForEach(tr =>
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
