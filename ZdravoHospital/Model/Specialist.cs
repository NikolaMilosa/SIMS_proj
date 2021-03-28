using System;
using System.Collections.Generic;

namespace Model
{
    public class Specialist : Doctor
    {

        public string SpecialistType { get; set; }

        public System.Collections.Generic.List<Operation> operation;

        public Specialist(string name, string surname, string email, DateTime dateOfBirth, string phoneNumber, string username, string parentsName, MaritalStatus maritalStatus, Gender gender, string specialistType) : base(name, surname, email, dateOfBirth, phoneNumber, username, parentsName, maritalStatus, gender)
        {
            SpecialistType = specialistType;
        }

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
                operation = value;
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
                newOperation.Specialist = this;
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
                    oldOperation.Specialist = null;
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
                    oldOperation.Specialist = null;
                tmpOperation.Clear();
            }
        }
    }
}