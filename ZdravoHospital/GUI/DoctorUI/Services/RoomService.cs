using Model;
using Repository.RoomPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ZdravoHospital.GUI.DoctorUI.Services
{
    public class RoomService
    {
        private RoomRepository _roomRepository;

        public RoomService()
        {
            _roomRepository = new RoomRepository();
        }

        public List<Room> GetAppointmentRooms()
        {
            return _roomRepository.GetValues().Where(r => r.RoomType == RoomType.APPOINTMENT_ROOM).ToList();
        }

        public List<Room> GetOperationRooms()
        {
            return _roomRepository.GetValues().Where(r => r.RoomType == RoomType.OPERATING_ROOM).ToList();
        }
    }
}
