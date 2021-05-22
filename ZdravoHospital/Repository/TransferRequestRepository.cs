using System;
using System.IO;
using System.Threading;

namespace Model.Repository
{
    public class TransferRequestRepository : Repository<int, TransferRequest>
    {
        private static string path = @"..\..\..\Resources\transferRequests.json";
        private static Mutex mutex;

        public TransferRequestRepository() : base(path)
        {
        }

        public override Mutex GetMutex()
        {
            if (mutex == null)
                mutex = new Mutex();

            return mutex;
        }

        public override TransferRequest GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(TransferRequest newValue)
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