using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Repository.PeriodPersistance
{
    public class PeriodRepository : IPeriodRepository
    {
        private static string _path = @"..\..\..\Resources\periods.json";

        public void Create(Period newValue)
        {
            var values = GetValues();
            if (values.Count - 1 >= 0)
                newValue.PeriodId = values[values.Count - 1].PeriodId + 1;
            else
                newValue.PeriodId = 0;
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(int id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.PeriodId == id);
            Save(values);
        }

        public Period GetById(int id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.PeriodId == id)
                {
                    return val;
                }
            }

            return null;
        }

        public List<Period> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Period>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<Period>();
            }

            return values;
        }

        public void Save(List<Period> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Period newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.PeriodId == newValue.PeriodId)] = newValue;
            Save(values);
        }
    }
}