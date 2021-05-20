using System;
using System.Collections.Generic;

namespace Model.Repository
{
   public interface IRepository<Value, Key>
   {
      void Save(List<Value> values);
      
      Value GetById(Key key);
      
      void DeleteById(Key key);
      
      void Update(Value newValue);
      
      List<Value> GetValues();
      
      void Create(Value newValue);
   
   }
}