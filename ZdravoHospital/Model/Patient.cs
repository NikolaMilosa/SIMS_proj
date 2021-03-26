using System;

namespace Model
{
   public class Patient : Person
   {
      public bool IsGuest { get; set; }
      public string HealthCardNumber { get; set; }

        public Patient(string healthCardNum, string name, string surname, string email, DateTime dateOfBirth, string phoneNumber, string username, string parentsName, MaritalStatus maritalStatus, Gender gender, string personID)
        {
            PName = name;
            Surname = surname;
            Email = email;
            DateOfBirth = dateOfBirth;
            PhoneNumber = phoneNumber;
            Username = username;
            ParentsName = parentsName;
            MaritalStatus = maritalStatus;
            Gender = gender;
            HealthCardNumber = healthCardNum;
            PersonID = personID;
        }

        public Patient(string name, string surname, string email, DateTime dateOfBirth, string phoneNumber, string username, string parentsName, MaritalStatus maritalStatus, Gender gender) : base(name, surname, email, dateOfBirth, phoneNumber, username, parentsName, maritalStatus, gender)
        {
        }

        public System.Collections.Generic.List<Appointment> appointment;
      
      public System.Collections.Generic.List<Appointment> Appointment
      {
         get
         {
            if (appointment == null)
               appointment = new System.Collections.Generic.List<Appointment>();
            return appointment;
         }
         set
         {
            RemoveAllAppointment();
            if (value != null)
            {
               foreach (Appointment oAppointment in value)
                  AddAppointment(oAppointment);
            }
         }
      }
      
      
      public void AddAppointment(Appointment newAppointment)
      {
         if (newAppointment == null)
            return;
         if (this.appointment == null)
            this.appointment = new System.Collections.Generic.List<Appointment>();
         if (!this.appointment.Contains(newAppointment))
         {
            this.appointment.Add(newAppointment);
            newAppointment.Patient = this;
         }
      }
      
      
      public void RemoveAppointment(Appointment oldAppointment)
      {
         if (oldAppointment == null)
            return;
         if (this.appointment != null)
            if (this.appointment.Contains(oldAppointment))
            {
               this.appointment.Remove(oldAppointment);
               oldAppointment.Patient = null;
            }
      }
      
      
      public void RemoveAllAppointment()
      {
         if (appointment != null)
         {
            System.Collections.ArrayList tmpAppointment = new System.Collections.ArrayList();
            foreach (Appointment oldAppointment in appointment)
               tmpAppointment.Add(oldAppointment);
            appointment.Clear();
            foreach (Appointment oldAppointment in tmpAppointment)
               oldAppointment.Patient = null;
            tmpAppointment.Clear();
         }
      }
      public System.Collections.Generic.List<Operation> operation;


        public System.Collections.Generic.List<Operation> Operation
      {
         get
         {
            if (operation == null)
               operation = new System.Collections.Generic.List<Operation>();
            return operation;
         }
         set
         {
            RemoveAllOperation();
            if (value != null)
            {
               foreach (Operation oOperation in value)
                  AddOperation(oOperation);
            }
         }
      }
      
      
      public void AddOperation(Operation newOperation)
      {
         if (newOperation == null)
            return;
         if (this.operation == null)
            this.operation = new System.Collections.Generic.List<Operation>();
         if (!this.operation.Contains(newOperation))
         {
            this.operation.Add(newOperation);
            newOperation.Patient = this;
         }
      }
      
      
      public void RemoveOperation(Operation oldOperation)
      {
         if (oldOperation == null)
            return;
         if (this.operation != null)
            if (this.operation.Contains(oldOperation))
            {
               this.operation.Remove(oldOperation);
               oldOperation.Patient = null;
            }
      }
      
      
      public void RemoveAllOperation()
      {
         if (operation != null)
         {
            System.Collections.ArrayList tmpOperation = new System.Collections.ArrayList();
            foreach (Operation oldOperation in operation)
               tmpOperation.Add(oldOperation);
            operation.Clear();
            foreach (Operation oldOperation in tmpOperation)
               oldOperation.Patient = null;
            tmpOperation.Clear();
         }
      }
   
   }
}