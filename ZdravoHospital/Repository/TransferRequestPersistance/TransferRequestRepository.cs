using Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Newtonsoft.Json;

namespace Repository.TransferRequestPersistance
{
    public class TransferRequestRepository : ITransferRequestRepository
    {
        private static string _path = @"..\..\..\Resources\transferRequests.json";
        private static Mutex _mutex;

        public TransferRequestRepository()
        {
        }

        private Mutex GetMutex()
        {
            if (_mutex == null)
                _mutex = new Mutex();

            return _mutex;
        }

        public void Create(TransferRequest newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.Add(newValue);
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public TransferRequest GetById(int id)
        {
            throw new NotImplementedException();
        }

        public List<TransferRequest> GetValues()
        {
            GetMutex().WaitOne();
            var values = JsonConvert.DeserializeObject<List<TransferRequest>>(File.ReadAllText(_path));
            if (values == null)
            {
                values = new List<TransferRequest>();
            }
            GetMutex().ReleaseMutex();
            return values;
        }

        public void Save(List<TransferRequest> values)
        {
            File.WriteAllText(_path,JsonConvert.SerializeObject(values,Formatting.Indented));
        }

        public void Update(TransferRequest newValue)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values[values.FindIndex(val => val.SenderRoom == newValue.SenderRoom &&
                                           val.RecipientRoom == newValue.RecipientRoom &&
                                           val.InventoryId.Equals(newValue.InventoryId) &&
                                           val.TimeOfExecution.Equals(newValue.TimeOfExecution))] = newValue;
            Save(values);
            GetMutex().ReleaseMutex();
        }

        public void DeleteByEquality(TransferRequest transferRequest)
        {
            var values = GetValues();
            GetMutex().WaitOne();
            values.RemoveAll(val => val.SenderRoom == transferRequest.SenderRoom &&
                                    val.RecipientRoom == transferRequest.RecipientRoom &&
                                    val.InventoryId.Equals(transferRequest.InventoryId) &&
                                    val.TimeOfExecution.Equals(transferRequest.TimeOfExecution) &&
                                    val.Quantity == transferRequest.Quantity);
            Save(values);
            GetMutex().ReleaseMutex();
        }
    }
}