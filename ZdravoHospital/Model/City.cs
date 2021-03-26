using System;

namespace Model
{
   public class City
   {
        private int postalCode;
        private string name;
        public Country country;

      public City(int postalCode, string name, Country country)
      {
            this.postalCode = postalCode;
            this.name = name;
            this.country = country;
      }

        public System.Collections.Generic.List<Adress> adress;
      
      public System.Collections.Generic.List<Adress> Adress
      {
         get
         {
            if (adress == null)
               adress = new System.Collections.Generic.List<Adress>();
            return adress;
         }
         set
         {
            RemoveAllAdress();
            if (value != null)
            {
               foreach (Adress oAdress in value)
                  AddAdress(oAdress);
            }
         }
      }
      
      
      public void AddAdress(Adress newAdress)
      {
         if (newAdress == null)
            return;
         if (this.adress == null)
            this.adress = new System.Collections.Generic.List<Adress>();
         if (!this.adress.Contains(newAdress))
         {
            this.adress.Add(newAdress);
            newAdress.City = this;
         }
      }
      
      
      public void RemoveAdress(Adress oldAdress)
      {
         if (oldAdress == null)
            return;
         if (this.adress != null)
            if (this.adress.Contains(oldAdress))
            {
               this.adress.Remove(oldAdress);
               oldAdress.City = null;
            }
      }
      
      
      public void RemoveAllAdress()
      {
         if (adress != null)
         {
            System.Collections.ArrayList tmpAdress = new System.Collections.ArrayList();
            foreach (Adress oldAdress in adress)
               tmpAdress.Add(oldAdress);
            adress.Clear();
            foreach (Adress oldAdress in tmpAdress)
               oldAdress.City = null;
            tmpAdress.Clear();
         }
      }
      
      
      
      public Country Country
      {
         get
         {
            return country;
         }
         set
         {
            if (this.country == null || !this.country.Equals(value))
            {
               if (this.country != null)
               {
                  Country oldCountry = this.country;
                  this.country = null;
                  oldCountry.RemoveCity(this);
               }
               if (value != null)
               {
                  this.country = value;
                  this.country.AddCity(this);
               }
            }
         }
      }

        public int PostalCode { get => postalCode; set => postalCode = value; }
        public string PName { get => name; set => name = value; }
    }
}