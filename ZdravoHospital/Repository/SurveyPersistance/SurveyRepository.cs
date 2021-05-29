using Model;
using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;

namespace Repository.SurveyPersistance
{
    public class SurveyRepository : ISurveyRepository
    {
        private static string _path = @"..\..\..\Resources\surveys.json";
        public void Create(Survey newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            throw new NotImplementedException();
        }

        public Survey GetById(string id)
        {
            throw new NotImplementedException();
        }

        public List<Survey> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Survey>>(File.ReadAllText(_path)) ?? new List<Survey>();

            return values;
        }

        public void Save(List<Survey> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Survey newValue)
        {
            throw new NotImplementedException();
        }
    }
}