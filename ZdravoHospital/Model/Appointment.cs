using System;

namespace Model
{
   public class Appointment
   {
      public DateTime startTime;
      public double duration;
      
      public Patient patient;

        public Appointment(DateTime startTime, double duration, Patient patient, Doctor doctor, AppointmentRoom appointmentRoom)
        {
            this.startTime = startTime;
            this.duration = duration;
            this.patient = patient;
            this.doctor = doctor;
            this.appointmentRoom = appointmentRoom;
        }

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
                  oldPatient.RemoveAppointment(this);
               }
               if (value != null)
               {
                  this.patient = value;
                  this.patient.AddAppointment(this);
               }
            }
         }
      }
      public Doctor doctor;
      
      
      public Doctor Doctor
      {
         get
         {
            return doctor;
         }
         set
         {
            if (this.doctor == null || !this.doctor.Equals(value))
            {
               if (this.doctor != null)
               {
                  Doctor oldDoctor = this.doctor;
                  this.doctor = null;
                  oldDoctor.RemoveAppointment(this);
               }
               if (value != null)
               {
                  this.doctor = value;
                  this.doctor.AddAppointment(this);
               }
            }
         }
      }
      public AppointmentRoom appointmentRoom;
      
      
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
   
   }
}