using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.RoomPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class RoomService
    {
        public RoomRepository RoomRepository { get; private set; }

        public RoomService()
        {
            RoomRepository = new RoomRepository();
        }
        public List<Room> GetAll() => RoomRepository.GetValues();
    }
}
