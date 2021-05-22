using Model;
using System;

namespace Repository.TransferRequestPersistance
{
   public interface ITransferRequestRepository : IRepository<int, TransferRequest>
   {
       void DeleteByEquality(TransferRequest transferRequest);
   }
}