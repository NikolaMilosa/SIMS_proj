using System;

namespace Model
{
   public class Country
   {
      public string name;

      public Country(string name)
      {
            this.name = name;
      }

        public System.Collections.Generic.List<City> city;
      
      public System.Collections.Generic.List<City> City
      {
         get
         {
            if (city == null)
               city = new System.Collections.Generic.List<City>();
            return city;
         }
         set
         {
            RemoveAllCity();
            if (value != null)
            {
               foreach (City oCity in value)
                  AddCity(oCity);
            }
         }
      }
      
      
      public void AddCity(City newCity)
      {
         if (newCity == null)
            return;
         if (this.city == null)
            this.city = new System.Collections.Generic.List<City>();
         if (!this.city.Contains(newCity))
         {
            this.city.Add(newCity);
            newCity.Country = this;
         }
      }
      
      
      public void RemoveCity(City oldCity)
      {
         if (oldCity == null)
            return;
         if (this.city != null)
            if (this.city.Contains(oldCity))
            {
               this.city.Remove(oldCity);
               oldCity.Country = null;
            }
      }
      
      
      public void RemoveAllCity()
      {
         if (city != null)
         {
            System.Collections.ArrayList tmpCity = new System.Collections.ArrayList();
            foreach (City oldCity in city)
               tmpCity.Add(oldCity);
            city.Clear();
            foreach (City oldCity in tmpCity)
               oldCity.Country = null;
            tmpCity.Clear();
         }
      }
   
   }
}