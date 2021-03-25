using System;

namespace Model
{
   public class Specialist : Doctor
   {
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
      public System.Collections.Generic.List<SpecialistType> specialistType;
      
      public System.Collections.Generic.List<SpecialistType> SpecialistType
      {
         get
         {
            if (specialistType == null)
               specialistType = new System.Collections.Generic.List<SpecialistType>();
            return specialistType;
         }
         set
         {
            RemoveAllSpecialistType();
            if (value != null)
            {
               foreach (SpecialistType oSpecialistType in value)
                  AddSpecialistType(oSpecialistType);
            }
         }
      }
      
      
      public void AddSpecialistType(SpecialistType newSpecialistType)
      {
         if (newSpecialistType == null)
            return;
         if (this.specialistType == null)
            this.specialistType = new System.Collections.Generic.List<SpecialistType>();
         if (!this.specialistType.Contains(newSpecialistType))
            this.specialistType.Add(newSpecialistType);
      }
      
      
      public void RemoveSpecialistType(SpecialistType oldSpecialistType)
      {
         if (oldSpecialistType == null)
            return;
         if (this.specialistType != null)
            if (this.specialistType.Contains(oldSpecialistType))
               this.specialistType.Remove(oldSpecialistType);
      }
      
      
      public void RemoveAllSpecialistType()
      {
         if (specialistType != null)
            specialistType.Clear();
      }
   
   }
}