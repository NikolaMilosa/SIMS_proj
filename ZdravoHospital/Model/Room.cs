using System;
using System.Collections.Generic;

namespace Model
{
    public class Room
    {
        public RoomType RoomType{ get; set; }
        public int Id{ get; set; }
        public string Name{ get; set; }
        public bool Avaliabe{ get; set; }

        public string AvaliableText 
        { 
            get { return Avaliabe ? "Yes" : "No"; }
        }

        public string RoomTypeText
        {
            get 
            {
                switch (RoomType)
                {
                    case RoomType.APPOINTMENT_ROOM:
                        return "Appointment";
                    case RoomType.OPERATING_ROOM:
                        return "Operation";
                    case RoomType.BREAK_ROOM:
                        return "Bedroom";
                    default:
                        return "Storage";
                }
            }
        }

        public System.Collections.Generic.List<Equipment> equipment;

        public System.Collections.Generic.List<Equipment> Equipment
        {
            get
            {
                if (equipment == null)
                    equipment = new System.Collections.Generic.List<Equipment>();
                return equipment;
            }
            set
            {
                RemoveAllEquipment();
                if (value != null)
                {
                    foreach (Equipment oEquipment in value)
                        AddEquipment(oEquipment);
                }
            }
        }


        public void AddEquipment(Equipment newEquipment)
        {
            if (newEquipment == null)
                return;
            if (this.equipment == null)
                this.equipment = new System.Collections.Generic.List<Equipment>();
            if (!this.equipment.Contains(newEquipment))
                this.equipment.Add(newEquipment);
        }


        public void RemoveEquipment(Equipment oldEquipment)
        {
            if (oldEquipment == null)
                return;
            if (this.equipment != null)
                if (this.equipment.Contains(oldEquipment))
                    this.equipment.Remove(oldEquipment);
        }


        public void RemoveAllEquipment()
        {
            if (equipment != null)
                equipment.Clear();
        }


        public Room(RoomType rt, int i, string n, bool a)
        {
            this.RoomType = rt;
            this.Id = i;
            this.Name = n;
            this.Avaliabe = a;
            this.equipment = new List<Equipment>();
        }
    }
}