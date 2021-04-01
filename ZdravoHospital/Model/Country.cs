using System;

namespace Model
{
   public class Country
   {
      public string Name { get; set; }
      public Country(string name)
      {
          this.Name = name;
      }
    }
}