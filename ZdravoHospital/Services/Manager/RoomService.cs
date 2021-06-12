using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using Model;
using Newtonsoft.Json.Linq;
using Repository.PeriodPersistance;
using Repository.RoomInventoryPersistance;
using Repository.RoomPersistance;
using ZdravoHospital.GUI.ManagerUI.DTOs;
using ZdravoHospital.GUI.ManagerUI.ViewModel;

namespace ZdravoHospital.Services.Manager
{
    public class RoomService
    {
        #region Repos

        private IRoomRepository _roomRepository;
        private IRoomInventoryRepository _roomInventoryRepository;
        private IPeriodRepository _periodRepository;

        #endregion

        #region Event things

        public delegate void RoomChangedEventHandler(object sender, EventArgs e);

        public event RoomChangedEventHandler RoomChanged;

        protected virtual void OnRoomChanged()
        {
            if (RoomChanged != null)
            {
                RoomChanged(this, EventArgs.Empty);
            }
        }

        #endregion

        public RoomService(InjectorDTO injector)
        {
            RoomChanged += ManagerWindowViewModel.GetDashboard().OnRoomsChanged;
            
            _roomRepository = injector.RoomRepository;
            _roomInventoryRepository = injector.RoomInventoryRepository;
            _periodRepository = injector.PeriodRepository;
        }

        public bool DeleteRoom(Room room)
        {
            if (!CheckIfPossible(room))
            {
                return false;
            }

            var roomsInventory = _roomInventoryRepository.FindAllInventoryInRoom(room.Id);

            if (roomsInventory.Count != 0)
            {
                var transportRoom = _roomRepository.FindRoomByPrio(room);
                if (transportRoom == null)
                {
                    return false;
                }

                TransportRoomInventory(room, transportRoom);
            }

            _roomRepository.DeleteById(room.Id);

            OnRoomChanged();
            return true;
        }

        public void AddRoom(Room room)
        {
            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            _roomRepository.Create(room);

            OnRoomChanged();
        }

        public void EditRoom(Room room)
        {
            room.Name = Regex.Replace(room.Name, @"\s+", " ");
            room.Name = room.Name.Trim();

            _roomRepository.Update(room);

            OnRoomChanged();
        }

        private void TransportRoomInventory(Room firstRoom, Room secondRoom)
        {
            var firstRoomInventory = _roomInventoryRepository.FindAllInventoryInRoom(firstRoom.Id);
            var secondRoomInventory = _roomInventoryRepository.FindAllInventoryInRoom(secondRoom.Id);

            foreach (var roomInventory in firstRoomInventory)
            {
                var existenceInSecondRoom = secondRoomInventory.Find(ri => ri.InventoryId.Equals(roomInventory.InventoryId));
                if (existenceInSecondRoom == null)
                {
                    /* doesn't exist there */
                    var newRoomInventory = new RoomInventory(roomInventory.InventoryId, secondRoom.Id, roomInventory.Quantity);
                    _roomInventoryRepository.Create(newRoomInventory);
                }
                else
                {
                    /* just edit its quantity */
                    _roomInventoryRepository.SetNewQuantity(existenceInSecondRoom, roomInventory.Quantity + existenceInSecondRoom.Quantity);
                }

                /* delete the reference */
                _roomInventoryRepository.DeleteByEquality(roomInventory);
            }
        }

        private bool CheckIfPossible(Room room)
        {
            if (room.RoomType == RoomType.BED_ROOM)
            {
                var periods = _periodRepository.GetValues();
                foreach (var period in periods)
                {
                    if (period.Treatment == null)
                    {
                        continue;
                    }

                    if (period.Treatment.StartDate > DateTime.Now && period.Treatment.RoomId == room.Id)
                    {
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
