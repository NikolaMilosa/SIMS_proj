using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Repository.SpecializationPersistance
{
    public class SpecializationRepository : ISpecializationRepository
    {
        private string _path = @"..\..\..\Resources\specializations.json";
        public void Create(Specialization newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.SpecializationName.Equals(id));
            Save(values);
        }

        public Specialization GetById(string id)
        {
            var values = GetValues();
            return values.Find(value => value.SpecializationName.Equals(id));
        }

        public List<Specialization> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Specialization>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Specialization>();
            }
            return values;
        }

        public void Save(List<Specialization> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Specialization newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.SpecializationName.Equals(newValue.SpecializationName))] = newValue;
            Save(values);
        }
    }
}