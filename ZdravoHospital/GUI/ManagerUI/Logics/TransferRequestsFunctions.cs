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
            Model.Resources.SerializeTransferRequests();

            GetTransferRequestMutex().ReleaseMutex();

            Task t = new Task(() => transferRequest.DoWork());
            t.Start();

            /* Create a roomSchedule for this transfer */
            RoomScheduleFunctions roomScheduleFunctions = new RoomScheduleFunctions();
            
            RoomSchedule roomScheduleSender = new RoomSchedule() { StartTime = transferRequest.TimeOfExecution, EndTime = transferRequest.TimeOfExecution.AddMinutes(2), RoomId = transferRequest.SenderRoom, ScheduleType = ReservationType.TRANSFER };
            roomScheduleFunctions.CreateAndScheduleRenovationStart(roomScheduleSender);

            RoomSchedule roomScheduleReciever = new RoomSchedule() { StartTime = transferRequest.TimeOfExecution.AddMinutes(2), EndTime = transferRequest.TimeOfExecution.AddMinutes(4), RoomId = transferRequest.RecipientRoom, ScheduleType = ReservationType.TRANSFER };
            roomScheduleFunctions.CreateAndScheduleRenovationStart(roomScheduleReciever);
        }

        public void ExecuteRequest(TransferRequest transferRequest)
        {
            if (Model.Resources.rooms.ContainsKey(transferRequest.SenderRoom) && Model.Resources.rooms.ContainsKey(transferRequest.RecipientRoom) && Model.Resources.inventory.ContainsKey(transferRequest.InventoryId))
            {
                RoomInventoryFunctions roomInventoryService = new RoomInventoryFunctions();
                /* Handle database transfer */
                RoomInventory sender = roomInventoryService.FindRoomInventoryByRoomAndInventory(transferRequest.SenderRoom, transferRequest.InventoryId);
                RoomInventory reciever = roomInventoryService.FindRoomInventoryByRoomAndInventory(transferRequest.RecipientRoom, transferRequest.InventoryId);

                if (sender.Quantity - transferRequest.Quantity == 0)
                {
                    roomInventoryService.DeleteByReference(sender);
                }
                else
                {
                    GetTransferRequestMutex().WaitOne();
                    sender.Quantity -= transferRequest.Quantity;
                    GetTransferRequestMutex().ReleaseMutex();
                }

                if (reciever == null)
                {
                    roomInventoryService.AddNewReference(new RoomInventory(transferRequest.InventoryId, transferRequest.RecipientRoom, transferRequest.Quantity));
                }
                else
                {
                    GetTransferRequestMutex().WaitOne();
                    reciever.Quantity += transferRequest.Quantity;
                    GetTransferRequestMutex().ReleaseMutex();
                }
            }

            GetTransferRequestMutex().WaitOne();
            /* Serialize */
            Model.Resources.SerializeRoomInventory();
            if (Model.Resources.transferRequests.Remove(transferRequest))
                Model.Resources.SerializeTransferRequests();

            if(ManagerWindow.dialog != null)
            {
                if (ManagerWindow.dialog.GetType().Name.Equals(nameof(InventoryManagementWindow)))
                {
                    InventoryManagementWindow activeWindow = (InventoryManagementWindow)ManagerWindow.dialog;
                    /* Update the visuals */

                    activeWindow.FirstRoom = activeWindow.FirstRoom;
                    activeWindow.SecondRoom = activeWindow.SecondRoom;
                }
            }
            GetTransferRequestMutex().ReleaseMutex();
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
