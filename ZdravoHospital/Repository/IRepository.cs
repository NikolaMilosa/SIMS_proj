using System;
using System.Collections.Generic;

namespace Repository
{
    public interface IRepository<TKey, TValue>
    {
        void Save(List<TValue> values);

        TValue GetById(TKey id);

        void DeleteById(TKey id);

        void Update(TValue newValue);

        List<TValue> GetValues();

        void Create(TValue newValue);

    }
}