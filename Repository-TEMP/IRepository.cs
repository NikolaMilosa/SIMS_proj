using System;

namespace Repository
{
   public interface IRepository<TKey,TValue>
   {
      void Save();
      
      TKey GetById(TKey id);
      
      void DeleteById(TKey id);
      
      void Update(TValue newValue);
      
      List<TKey> GetValues();
      
      void Create(TValue newValue);
   
   }
}