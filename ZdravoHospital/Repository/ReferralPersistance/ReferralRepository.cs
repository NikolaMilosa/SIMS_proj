using Model;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Repository.ReferralPersistance
{
    public class ReferralRepository : IReferralRepository
    {
        private static string _path = @"../../../Resources/referrals.json";

        public void Create(Referral newValue)
        {
            var values = GetValues();
            if (values.Count - 1 >= 0)
                newValue.ReferralId = values[values.Count - 1].ReferralId + 1;
            else
                newValue.ReferralId = 0;
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(int id)
        {
            var values = GetValues();
            values.RemoveAll(val => val.ReferralId == id);
            Save(values);
        }

        public Referral GetById(int id)
        {
            var values = GetValues();
            foreach (var val in values)
            {
                if (val.ReferralId == id)
                {
                    return val;
                }
            }

            return null;
        }

        public List<Referral> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Referral>>(File.ReadAllText(_path));

            if (values == null)
                values = new List<Referral>();

            return values;
        }

        public void Save(List<Referral> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Referral newValue)
        {
            var values = GetValues();
            values[values.FindIndex(val => val.ReferralId == newValue.ReferralId)] = newValue;
            Save(values);
        }
    }
}