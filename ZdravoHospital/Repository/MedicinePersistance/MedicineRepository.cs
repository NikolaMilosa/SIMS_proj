using Model;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace Repository.MedicinePersistance
{
    public class MedicineRepository : IMedicineRepository
    {
        private string _path = @"..\..\..\Resources\medicines.json";
        public void Create(Medicine newValue)
        {
            var values = GetValues();
            values.Add(newValue);
            Save(values);
        }

        public void DeleteById(string id)
        {
            var values = GetValues();
            values.RemoveAll(value => value.MedicineName.Equals(id));
            Save(values);
        }

        public Medicine GetById(string id)
        {
            List<Medicine> medicines = GetValues();
            foreach (Medicine medicine in medicines)
                if (medicine.MedicineName.Equals(id))
                    return medicine;

            return null;
        }

        public List<Medicine> GetValues()
        {
            var values = JsonConvert.DeserializeObject<List<Medicine>>(File.ReadAllText(_path));

            if (values == null)
            {
                values = new List<Medicine>();
            }

            return values;
        }

        public void Save(List<Medicine> values)
        {
            File.WriteAllText(_path, JsonConvert.SerializeObject(values, Formatting.Indented));
        }

        public void Update(Medicine newValue)
        {
            List<Medicine> medicines = GetValues();
            medicines[medicines.FindIndex(patient => patient.MedicineName.Equals(newValue.MedicineName))] = newValue;
            Save(medicines);
        }
    }
}