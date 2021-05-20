using Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class InventoryFunctions
    {
        private InventoryRepository _inventoryRepository;

        private static Mutex _inventoryMutex;

        public static Mutex GetInventoryMutex()
        {
            if (_inventoryMutex == null)
                _inventoryMutex = new Mutex();
            return _inventoryMutex;
        }

        #region Event Things

        public delegate void InventoryChangedEventHandler(object sender, EventArgs e);

        public event InventoryChangedEventHandler InventoryChanged;

        protected virtual void OnInventoryChanged()
        {
            if (InventoryChanged != null)
            {
                InventoryChanged(this, EventArgs.Empty);
            }
        }
        #endregion

        public InventoryFunctions()
        {
            InventoryChanged += ManagerWindowViewModel.GetDashboard().OnInventoryChanged;

            _inventoryRepository = new InventoryRepository();
        }

        public bool DeleteInventory(Inventory someInventory)
        {
            var roomInventoryService = new RoomInventoryFunctions();
            roomInventoryService.DeleteByInventoryId(someInventory.Id);

            _inventoryRepository.DeleteById(someInventory.Id);

            OnInventoryChanged();
            return true;
        }

        public bool AddInventory(Inventory newInventory)
        {
            GetInventoryMutex().WaitOne();
            var roomFunctions = new RoomFunctions();
            var roomInventoryFunctions = new RoomInventoryFunctions();

            var someRoom = roomFunctions.FindRoomByPrio(null);

            if (someRoom == null)
            {
                GetInventoryMutex().ReleaseMutex();
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
            _inventoryRepository.Create(newInventory);

            GetInventoryMutex().ReleaseMutex();
            roomInventoryFunctions.AddNewReference(new RoomInventory(newInventory.Id, someRoom.Id, newInventory.Quantity));

            OnInventoryChanged();
            return true;
        }

        public void EditInventory(Inventory newInventory)
        {
            GetInventoryMutex().WaitOne();
            
            /* Clean input */
            newInventory.Name = Regex.Replace(newInventory.Name, @"\s+", " ");
            newInventory.Name = newInventory.Name.Trim().ToLower();

            newInventory.Supplier = Regex.Replace(newInventory.Supplier, @"\s+", " ");
            newInventory.Supplier = newInventory.Supplier.Trim().ToLower();
            newInventory.Supplier = newInventory.Supplier.Substring(0, 1).ToUpper() + newInventory.Supplier.Substring(1);

            newInventory.Id = Regex.Replace(newInventory.Id, @"\s+", " ");
            newInventory.Id = newInventory.Id.Trim().ToUpper();

            _inventoryRepository.Update(newInventory);

            GetInventoryMutex().ReleaseMutex();
            OnInventoryChanged();
        }

        public void EditInventoryAmount(Inventory inventory, int newQuantity, Room room)
        {
            GetInventoryMutex().WaitOne();
            var roomInventoryFunctions = new RoomInventoryFunctions();
            int difference;

            var roomInventory = roomInventoryFunctions.FindRoomInventoryByRoomAndInventory(room.Id, inventory.Id);
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

                if (newQuantity == 0)
                {
                    roomInventoryFunctions.DeleteByReference(roomInventory);
                }
                else
                {
                    roomInventoryFunctions.SetNewQuantity(roomInventory, newQuantity);
                }
            }
            
            if (difference < 0)
            {
                difference = (-1) * difference;
                inventory.Quantity -= difference;
            }
            else
            {
                inventory.Quantity += difference;
            }

            GetInventoryMutex().ReleaseMutex();

            if (inventory.Quantity == 0)
            {
                DeleteInventory(inventory);
            }
            else
            {
                _inventoryRepository.Update(inventory);
            }

            OnInventoryChanged();
        }
    }
}
