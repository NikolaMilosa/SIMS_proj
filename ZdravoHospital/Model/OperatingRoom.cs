using System;

namespace Model
{
   public class OperatingRoom : Room
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
            newOperation.OperatingRoom = this;
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
               oldOperation.OperatingRoom = null;
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
               oldOperation.OperatingRoom = null;
            tmpOperation.Clear();
         }
      }
   
   }
}