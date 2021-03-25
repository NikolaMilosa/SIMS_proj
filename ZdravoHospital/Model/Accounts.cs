using System;

namespace Model
{
   public class Accounts
   {
      public System.Collections.Generic.List<Credentials> credentials;
      
      public System.Collections.Generic.List<Credentials> Credentials
      {
         get
         {
            if (credentials == null)
               credentials = new System.Collections.Generic.List<Credentials>();
            return credentials;
         }
         set
         {
            RemoveAllCredentials();
            if (value != null)
            {
               foreach (Credentials oCredentials in value)
                  AddCredentials(oCredentials);
            }
         }
      }
      
      
      public void AddCredentials(Credentials newCredentials)
      {
         if (newCredentials == null)
            return;
         if (this.credentials == null)
            this.credentials = new System.Collections.Generic.List<Credentials>();
         if (!this.credentials.Contains(newCredentials))
            this.credentials.Add(newCredentials);
      }
      
      
      public void RemoveCredentials(Credentials oldCredentials)
      {
         if (oldCredentials == null)
            return;
         if (this.credentials != null)
            if (this.credentials.Contains(oldCredentials))
               this.credentials.Remove(oldCredentials);
      }
      
      
      public void RemoveAllCredentials()
      {
         if (credentials != null)
            credentials.Clear();
      }
   
   }
}