using System;
using System.Collections.Generic;
using System.Text;
using Model;
using Repository.RoomPersistance;

namespace ZdravoHospital.GUI.PatientUI.Logics
{
    public class RoomFunctions
    {
        public RoomRepository RoomRepository { get; private set; }

        public RoomFunctions()
        {
            RoomRepository = new RoomRepository();
        }
        public List<Room> GetAll() => RoomRepository.GetValues();
    }
}
