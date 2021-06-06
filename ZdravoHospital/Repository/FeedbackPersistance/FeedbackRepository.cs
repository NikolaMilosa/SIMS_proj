using System;
using System.Collections.Generic;
using System.IO;
using Model;
using Newtonsoft.Json;

namespace Repository.FeedbackPersistance
{
    public class FeedbackRepository : IFeedbackRepository
    {
        private static string _path = @"..\..\..\Resources\feedback.json";

        public void Save(List<Feedback> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public Feedback GetById(Guid id)
        {
            var values = GetValues();
            return values.Find(val => val.Id.Equals(id));
        }

        public void DeleteById(Guid id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.Id.Equals(id));
            Save(values);
        }

        public void Update(Feedback newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.Id.Equals(newValue.Id))] = newValue;
            Save(values);
        }

        public List<Feedback> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Feedback>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Feedback>();
            }

            return values;
        }

        public void Create(Feedback newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }
    }
}