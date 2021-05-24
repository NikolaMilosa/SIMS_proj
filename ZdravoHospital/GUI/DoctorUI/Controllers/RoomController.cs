using Model;
using System.Collections.Generic;
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
    }
}
