using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows;
using Model;
using Model.Repository;
using ZdravoHospital.GUI.ManagerUI.DTOs;

namespace ZdravoHospital.GUI.ManagerUI.Logics
{
    public class RoomFunctions
    {
        private RoomRepository _roomRepository;
        
        #region Event Things

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

        public RoomFunctions()
        {
            RoomChanged += ViewModel.ManagerWindowViewModel.GetDashboard().OnRoomsChanged;
            _roomRepository = new RoomRepository();
        }

        private Room FindRoomByType(RoomType rt, Room room)
        {
            if (room != null)
            {
                foreach (var r in _roomRepository.GetValues())
                {
                    if (r.Available == true && r.RoomType == rt && r.Id != room.Id)
                        return r;
                }
            }
            else
            {
                foreach (var r in _roomRepository.GetValues())
                {
                    if (r.Available == true && r.RoomType == rt)
                        return r;
                }
            }
            

            return null;
        }

        public Room FindRoomByPrio(Room notThisRoom)
        {
            var someRoom = FindRoomByType(RoomType.STORAGE_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.BED_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.APPOINTMENT_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.OPERATING_ROOM, notThisRoom);

            if (someRoom == null)
                someRoom = FindRoomByType(RoomType.EMERGENCY_ROOM, notThisRoom);

            return someRoom;
        }

        public bool DeleteRoom(Room room)
        {
            /* First handle its inventory */
            var roomInventoryService = new RoomInventoryFunctions();
            var roomsInventory = roomInventoryService.FindAllInventoryInRoom(room.Id);

            if(roomsInventory.Count != 0)
            {
                var transportRoom = FindRoomByPrio(room);

                if (transportRoom == null)
                {
                    /* There are no rooms where this inventory would be transported */
                    return false;
                }

                /* Can be refactored when room transferring is added, this transfers from a room being deleted 
                 * other suitable room */

                roomInventoryService.TransportRoomInventory(room, transportRoom);
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

        public void ChangeRoomAvailability(int roomId, bool newValue)
        {
            var room = _roomRepository.GetById(roomId);
            room.Available = newValue;
           
            _roomRepository.Update(room);
            
            OnRoomChanged();
        }
    }
}
