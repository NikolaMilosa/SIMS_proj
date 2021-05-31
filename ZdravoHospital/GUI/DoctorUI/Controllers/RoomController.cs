using Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using ZdravoHospital.GUI.DoctorUI.Services;

namespace ZdravoHospital.GUI.DoctorUI.Controllers
{
    public class RoomController
    {
        private RoomService _rooomService;

        public RoomController()
        {
            _rooomService = new RoomService();
        }

        public List<Room> GetAppointmentRooms()
        {
            return _rooomService.GetAppointmentRooms();
        }

        public List<Room> GetOperationRooms()
        {
            return _rooomService.GetOperationRooms();
        }

        public List<Room> GetBedrooms()
        {
            return _rooomService.GetBedrooms();
        }
    }
}
