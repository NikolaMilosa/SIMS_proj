using System;
using System.Collections.Generic;
using System.Printing;
using System.Text;
using System.Text.RegularExpressions;
using Model;
using Repository.InventoryPersistance;
using Repository.PeriodPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class InventoryService
    {
        #region Repos

        private IRoomRepository _roomRepository;
        private IRoomInventoryRepository _roomInventoryRepository;
        private IInventoryRepository _inventoryRepository;
        private IPeriodRepository _periodRepository;

        #endregion

        #region Event things

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

        public InventoryService(InjectorDTO injector)
        {
            InventoryChanged += ManagerWindowViewModel.GetDashboard().OnInventoryChanged;
            
            _roomInventoryRepository = injector.RoomInventoryRepository;
            _roomRepository = injector.RoomRepository;
            _inventoryRepository = injector.InventoryRepository;
            _periodRepository = injector.PeriodRepository;
        }

        public bool DeleteInventory(Inventory inventory)
        {
            if (!CheckIfPossible(inventory))
            {
                return false;
            }

            _roomInventoryRepository.DeleteByInventoryId(inventory.Id);
            _inventoryRepository.DeleteById(inventory.Id);

            var notification = new MyMessageBoxViewModel(
                $"Inventory with id {inventory.Id} has successfully been deleted! Main grid will update after " +
                $"this dialog is closed.");

            OnInventoryChanged();

            return true;
        }

        public bool AddInventory(Inventory newInventory)
        {
            var room = _roomRepository.FindRoomByPrio(null);

            if (room == null)
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
            _inventoryRepository.Create(newInventory);
            _roomInventoryRepository.Create(new RoomInventory(newInventory.Id, room.Id, newInventory.Quantity));

            var notification = new MyMessageBoxViewModel(
                $"Inventory with id {newInventory.Id} has successfully been added! Main data grid will" +
                $" update after this dialog is closed.");

            OnInventoryChanged();

            return true;
        }

        public void EditInventory(Inventory newInventory)
        {
            /* Clean input */
            newInventory.Name = Regex.Replace(newInventory.Name, @"\s+", " ");
            newInventory.Name = newInventory.Name.Trim().ToLower();

            newInventory.Supplier = Regex.Replace(newInventory.Supplier, @"\s+", " ");
            newInventory.Supplier = newInventory.Supplier.Trim().ToLower();
            newInventory.Supplier = newInventory.Supplier.Substring(0, 1).ToUpper() + newInventory.Supplier.Substring(1);

            newInventory.Id = Regex.Replace(newInventory.Id, @"\s+", " ");
            newInventory.Id = newInventory.Id.Trim().ToUpper();

            _inventoryRepository.Update(newInventory);

            var notification = new MyMessageBoxViewModel(
                $"Inventory with id {newInventory.Id} has successfully been updated! Main data grid " +
                $"will update after this dialog is closed.");

            OnInventoryChanged();
        }

        public void EditInventoryAmount(Inventory inventory, int newQuantity, Room room)
        {
            int difference;

            var roomInventory = _roomInventoryRepository.FindByBothIds(room.Id, inventory.Id);
            if (roomInventory == null)
            {
                _roomInventoryRepository.Create(new RoomInventory(inventory.Id, room.Id, newQuantity));
                difference = newQuantity;
            }
            else
            {
                if (newQuantity == roomInventory.Quantity)
                    return;

                difference = newQuantity - roomInventory.Quantity;
                if (newQuantity == 0)
                {
                    _roomInventoryRepository.DeleteByEquality(roomInventory);
                }
                else
                {
                    _roomInventoryRepository.SetNewQuantity(roomInventory,newQuantity);
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

            if (inventory.Quantity == 0)
            {
                _inventoryRepository.DeleteById(inventory.Id);
            }
            else
            {
                _inventoryRepository.Update(inventory);
            }

            var notification = new MyMessageBoxViewModel(
                $"Inventory with id {inventory.Id} has had its amount successfully changed! Main data" +
                $" grid will update after this dialog is closed.");

            OnInventoryChanged();
        }

        private bool CheckIfPossible(Inventory inv)
        {
            if (inv.Name.ToLower().Equals("bed"))
            {
                var periods = _periodRepository.GetValues();
                foreach (var period in periods)
                {
                    if (period.Treatment == null)
                    {
                        continue;
                    }

                    if (period.Treatment.StartDate > DateTime.Now)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
