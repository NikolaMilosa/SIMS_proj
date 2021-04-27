using System;

namespace Model
{
   /// - Resembles hospital mark
   /// - Is annonymous
   public class Survey
   {
      public string PatientUsername { get; set; }
      public DateTime CreationDate { get; set; }
      public int AppointmentAccessibility{get; set;}
      public int Care{get; set;}
      public int Recommendation{get; set;}
      public int Hygiene{get; set;}
      public string Comment{get; set;}
   
   }
}