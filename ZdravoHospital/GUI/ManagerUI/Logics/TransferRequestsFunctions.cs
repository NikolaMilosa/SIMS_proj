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

        public static void StartTransfer(TransferRequest tr)
        {
            Task t = new Task(() => tr.DoWork());
            t.Start();
        }

        public static void CreateAndStartTransfer(TransferRequest tr)
        {
            Model.Resources.transferRequests.Add(tr);
            Model.Resources.SerializeTransferRequests();
            Task t = new Task(() => tr.DoWork());
            t.Start();
        }

        public static void ExecuteRequest(TransferRequest tr)
        {
            if (Model.Resources.rooms.ContainsKey(tr.SenderRoom) && Model.Resources.rooms.ContainsKey(tr.RecipientRoom) && Model.Resources.inventory.ContainsKey(tr.InventoryId))
            {
                /* Handle database transfer */
                RoomInventory sender = RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(tr.SenderRoom, tr.InventoryId);
                RoomInventory reciever = RoomInventoryFunctions.FindRoomInventoryByRoomAndInventory(tr.RecipientRoom, tr.InventoryId);

                if (sender.Quantity - tr.Quantity == 0)
                {
                    RoomInventoryFunctions.DeleteByReference(sender);
                }
                else
                {
                    sender.Quantity -= tr.Quantity;
                }

                if (reciever == null)
                {
                    RoomInventoryFunctions.AddNewReference(new RoomInventory(tr.InventoryId, tr.RecipientRoom, tr.Quantity));
                }
                else
                {
                    reciever.Quantity += tr.Quantity;
                }
            }

            /* Serialize */
            Model.Resources.SerializeRoomInventory();
            if (Model.Resources.transferRequests.Remove(tr))
                Model.Resources.SerializeTransferRequests();

            if(ManagerWindow.dialog != null)
            {
                if (ManagerWindow.dialog.GetType().Name.Equals(nameof(InventoryManagementWindow)))
                {
                    InventoryManagementWindow activeWindow = (InventoryManagementWindow)ManagerWindow.dialog;
                    /* Update the visuals */
                    Room tempRoomSender = activeWindow.FirstRoom;
                    Room tempRoomReciever = activeWindow.SecondRoom;

                    activeWindow.FirstRoom = tempRoomReciever;
                    activeWindow.SecondRoom = tempRoomSender;

                    activeWindow.FirstRoom = tempRoomSender;
                    activeWindow.SecondRoom = tempRoomReciever;
                }
            }
        }
    }
}
