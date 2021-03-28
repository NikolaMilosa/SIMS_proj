using System;

namespace Model
{
    public class Appointment : Period
    {
        public Appointment() : base()
        {
            PeriodType = PeriodType.APPOINTMENT;
        }


        //public Patient patient;

        //public Patient Patient
        //{
        //    get
        //    {
        //        return patient;
        //    }
        //    set
        //    {
        //        if (this.patient == null || !this.patient.Equals(value))
        //        {
        //            if (this.patient != null)
        //            {
        //                Patient oldPatient = this.patient;
        //                this.patient = null;
        //                oldPatient.RemoveAppointment(this);
        //            }
        //            if (value != null)
        //            {
        //                this.patient = value;
        //                this.patient.AddAppointment(this);
        //            }
        //        }
        //    }
        //}

        public string PatientUsername { get; set; }

        //public Doctor doctor;

        //public Doctor Doctor
        //{
        //    get
        //    {
        //        return doctor;
        //    }
        //    set
        //    {
        //        if (this.doctor == null || !this.doctor.Equals(value))
        //        {
        //            if (this.doctor != null)
        //            {
        //                Doctor oldDoctor = this.doctor;
        //                this.doctor = null;
        //                oldDoctor.RemoveAppointment(this);
        //            }
        //            if (value != null)
        //            {
        //                this.doctor = value;
        //                this.doctor.AddAppointment(this);
        //            }
        //        }
        //    }
        //}

        public string DoctorUsername { get; set; }

        public AppointmentRoom appointmentRoom;
        private DateTime dateTime;
        private int v;
        private Patient p;
        private Doctor d;
        private AppointmentRoom ap;

        public Appointment(DateTime dateTime, int v, Patient p, Doctor d, AppointmentRoom ap)
        {
            this.DateTime = dateTime;
            this.V = v;
            this.P = p;
            this.D = d;
            this.Ap = ap;
        }

        public Appointment()
        {
        }

        public AppointmentRoom AppointmentRoom
        {
            get
            {
                return appointmentRoom;
            }
            set
            {
                if (this.appointmentRoom == null || !this.appointmentRoom.Equals(value))
                {
                    if (this.appointmentRoom != null)
                    {
                        AppointmentRoom oldAppointmentRoom = this.appointmentRoom;
                        this.appointmentRoom = null;
                        oldAppointmentRoom.RemoveAppointment(this);
                    }
                    if (value != null)
                    {
                        this.appointmentRoom = value;
                        this.appointmentRoom.AddAppointment(this);
                    }
                }
            }
        }

        public DateTime DateTime { get => dateTime; set => dateTime = value; }
        public int V { get => V1; set => V1 = value; }
        public int V1 { get => v; set => v = value; }
        public Patient P { get => P1; set => P1 = value; }
        public Patient P1 { get => p; set => p = value; }
        public Doctor D { get => d; set => d = value; }
        public AppointmentRoom Ap { get => ap; set => ap = value; }
    }
}