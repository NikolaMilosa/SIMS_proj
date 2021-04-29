using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class InventoryFunctions
    {
        private static Mutex inventoryMutex;

        public static Mutex GetInventoryMutex()
        {
            if (inventoryMutex == null)
                inventoryMutex = new Mutex();
            return inventoryMutex;
        }

        public InventoryFunctions() { }

        public bool DeleteInventory(Inventory someInventory)
        {
            GetInventoryMutex().WaitOne();

            RoomInventoryFunctions roomInventoryService = new RoomInventoryFunctions();
            roomInventoryService.DeleteByInventoryId(someInventory.Id);

            /* Visual */
            ManagerWindow.Inventory.Remove(someInventory);

            Model.Resources.inventory.Remove(someInventory.Id);
            Model.Resources.SerializeInventory();

            GetInventoryMutex().ReleaseMutex();
            return true;
        }

        public bool AddInventory(Inventory newInventory)
        {
            RoomFunctions roomFunctions = new RoomFunctions();
            RoomInventoryFunctions roomInventoryFunctions = new RoomInventoryFunctions();

            Room someRoom = roomFunctions.FindRoomByPrio(null);

            if (someRoom == null)
            {
                return false;
            }
            /* Clean input */
            newInventory.Name = Regex.Replace(newInventory.Name, @"\s+", " ");
            newInventory.Name = newInventory.Name.Trim().ToLower();

            newInventory.Supplier = Regex.Replace(newInventory.Supplier, @"\s+", " ");
            newInventory.Supplier = newInventory.Supplier.Trim().ToLower();
            newInventory.Supplier = newInventory.Supplier.Substring(0, 1).ToUpper() + newInventory.Supplier.Substring(1);

            newInventory.Id = Regex.Replace(newInventory.Id, @"\s+", " ");
            newInventory.Id = newInventory.Id.Trim().ToUpper();

            /* Found a room to put some inventory in */
            GetInventoryMutex().WaitOne();

            Model.Resources.inventory[newInventory.Id] = newInventory;
            roomInventoryFunctions.AddNewReference(new RoomInventory(newInventory.Id, someRoom.Id, newInventory.Quantity));

            Model.Resources.SerializeInventory();
            Model.Resources.SerializeRoomInventory();

            ManagerWindow.Inventory.Add(Model.Resources.inventory[newInventory.Id]);

            GetInventoryMutex().ReleaseMutex();
            return true;
        }

        public void EditInventory(Inventory oldInventory, Inventory newInventory)
        {
            GetInventoryMutex().WaitOne();

            int index = ManagerWindow.Inventory.IndexOf(oldInventory);
            ManagerWindow.Inventory.Remove(oldInventory);

            /* Clean input */
            newInventory.Name = Regex.Replace(newInventory.Name, @"\s+", " ");
            newInventory.Name = newInventory.Name.Trim().ToLower();

            newInventory.Supplier = Regex.Replace(newInventory.Supplier, @"\s+", " ");
            newInventory.Supplier = newInventory.Supplier.Trim().ToLower();
            newInventory.Supplier = newInventory.Supplier.Substring(0, 1).ToUpper() + newInventory.Supplier.Substring(1);

            newInventory.Id = Regex.Replace(newInventory.Id, @"\s+", " ");
            newInventory.Id = newInventory.Id.Trim().ToUpper();

            Model.Resources.inventory[oldInventory.Id] = newInventory;

            ManagerWindow.Inventory.Insert(index, newInventory);

            Model.Resources.SerializeInventory();

            GetInventoryMutex().ReleaseMutex();
        }

        public void EditInventoryAmount(Inventory inventory, int newQuantity, Room room)
        {
            RoomInventoryFunctions roomInventoryFunctions = new RoomInventoryFunctions();

            int difference;

            RoomInventory roomInventory = roomInventoryFunctions.FindRoomInventoryByRoomAndInventory(room.Id, inventory.Id);
            if (roomInventory == null)
            {
                roomInventoryFunctions.AddNewReference(new RoomInventory(inventory.Id, room.Id, newQuantity));
                difference = newQuantity;
            }
            else
            {
                if (newQuantity == roomInventory.Quantity)
                    return;

                difference = newQuantity - roomInventory.Quantity;
                roomInventory.Quantity = newQuantity;

                if (newQuantity == 0)
                {
                    roomInventoryFunctions.DeleteByReference(roomInventory);
                }
            }

            GetInventoryMutex().WaitOne();

            int index = ManagerWindow.Inventory.IndexOf(inventory);
            ManagerWindow.Inventory.Remove(inventory);

            if (difference < 0)
            {
                difference = (-1) * difference;
                Model.Resources.inventory[inventory.Id].Quantity -= difference;
                if (Model.Resources.inventory[inventory.Id].Quantity == 0)
                    Model.Resources.inventory.Remove(inventory.Id);
            }
            else
            {
                Model.Resources.inventory[inventory.Id].Quantity += difference;
            }
            
            ManagerWindow.Inventory.Insert(index, inventory);

            Model.Resources.SerializeRoomInventory();
            Model.Resources.SerializeInventory();

            GetInventoryMutex().ReleaseMutex();
        }
    }
}
