using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Model;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public static class TransferRequestsFunctions
    {
        public static void RunOrExecute()
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

        public static void StartTransfer(TransferRequest transferRequest)
        {
            Task t = new Task(() => transferRequest.DoWork());
            t.Start();
        }

        public static void CreateAndStartTransfer(TransferRequest transferRequest)
        {
            Model.Resources.transferRequests.Add(transferRequest);
            Model.Resources.SerializeTransferRequests();
            Task t = new Task(() => transferRequest.DoWork());
            t.Start();
        }

        public static void ExecuteRequest(TransferRequest transferRequest)
        {
            if (Model.Resources.rooms.ContainsKey(transferRequest.SenderRoom) && Model.Resources.rooms.ContainsKey(transferRequest.RecipientRoom) && Model.Resources.inventory.ContainsKey(transferRequest.InventoryId))
            {
                /* Handle database transfer */
                RoomInventory sender = RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(transferRequest.SenderRoom, transferRequest.InventoryId);
                RoomInventory reciever = RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(transferRequest.RecipientRoom, transferRequest.InventoryId);

                if (sender.Quantity - transferRequest.Quantity == 0)
                {
                    RoomInventoryFunctions.DeleteByReference(sender);
                }
                else
                {
                    sender.Quantity -= transferRequest.Quantity;
                }

                if (reciever == null)
                {
                    RoomInventoryFunctions.AddNewReference(new RoomInventory(transferRequest.InventoryId, transferRequest.RecipientRoom, transferRequest.Quantity));
                }
                else
                {
                    reciever.Quantity += transferRequest.Quantity;
                }
            }

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
        }
    }
}
