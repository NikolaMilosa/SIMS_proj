using System;
using System.IO;

namespace Model.Repository
{
   public class TransferRequestRepository : Repository<int,TransferRequest>
   {
       private static string path = @"..\..\..\Resources\transferRequests.json";

       public TransferRequestRepository() : base(path)
       {
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
           throw new NotImplementedException();
       }
   }
}