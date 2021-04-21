using Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public static class InventoryFunctions
    {
        public static bool DeleteInventory(Inventory someInventory)
        {
            RoomInventoryFunctions.DeleteByInventoryId(someInventory.Id);

            /* Visual */
            ManagerWindow.Inventory.Remove(someInventory);


            Model.Resources.inventory.Remove(someInventory.Id);
            Model.Resources.SerializeInventory();
            return true;
        }
    }
}
