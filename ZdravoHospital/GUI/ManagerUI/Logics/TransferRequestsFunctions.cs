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
        public static void CreateAndStartTransfer(TransferRequest tr)
        {
            Model.Resources.transferRequests.Add(tr);
            Model.Resources.SerializeTransferRequests();
            Task t = new Task(() => tr.DoWork());
            t.Start();
        }

        public static void ExecuteRequest(TransferRequest tr)
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

            /* Delete this transferRequest */
            Model.Resources.transferRequests.Remove(tr);

            /* Serialize */
            Model.Resources.SerializeRoomInventory();
            Model.Resources.SerializeTransferRequests();

            if (ManagerWindow.dialog.GetType().Name.Equals(nameof(InventoryManagementWindow)))
            {
                InventoryManagementWindow activeWindow = (InventoryManagementWindow)ManagerWindow.dialog;
                /* Update the visuals */
                Room helper = activeWindow.FirstRoom;
                activeWindow.FirstRoom = activeWindow.SecondRoom;
                activeWindow.SecondRoom = helper;

                activeWindow.SecondRoom = activeWindow.FirstRoom;
                activeWindow.FirstRoom = helper;
            }
        }
    }
}
