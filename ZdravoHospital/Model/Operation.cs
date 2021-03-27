using System;

namespace Model
{
    public class Operation : Period
    {
        public Specialist specialist;

        public Specialist Specialist
        {
            get
            {
                return specialist;
            }
            set
            {
                if (this.specialist == null || !this.specialist.Equals(value))
                {
                    if (this.specialist != null)
                    {
                        Specialist oldSpecialist = this.specialist;
                        this.specialist = null;
                        oldSpecialist.RemoveOperation(this);
                    }
                    if (value != null)
                    {
                        this.specialist = value;
                        this.specialist.AddOperation(this);
                    }
                }
            }
        }
        public Patient patient;

        public Patient Patient
        {
            get
            {
                return patient;
            }
            set
            {
                if (this.patient == null || !this.patient.Equals(value))
                {
                    if (this.patient != null)
                    {
                        Patient oldPatient = this.patient;
                        this.patient = null;
                        oldPatient.RemoveOperation(this);
                    }
                    if (value != null)
                    {
                        this.patient = value;
                        this.patient.AddOperation(this);
                    }
                }
            }
        }
        public OperatingRoom operatingRoom;

        public OperatingRoom OperatingRoom
        {
            get
            {
                return operatingRoom;
            }
            set
            {
                if (this.operatingRoom == null || !this.operatingRoom.Equals(value))
                {
                    if (this.operatingRoom != null)
                    {
                        OperatingRoom oldOperatingRoom = this.operatingRoom;
                        this.operatingRoom = null;
                        oldOperatingRoom.RemoveOperation(this);
                    }
                    if (value != null)
                    {
                        this.operatingRoom = value;
                        this.operatingRoom.AddOperation(this);
                    }
                }
            }
        }

    }
}