using System;
using System.Threading;

namespace Model.Repository
{
   public class SurveyRepository : Repository<string,Survey>
   {
       private static string path = @"..\..\..\Resources\surveys.json";

       public SurveyRepository() : base(path)
       {
       }

       public override Mutex GetMutex()
       {
           return new Mutex();
       }

       public override Survey GetById(string id)
       {
           throw new NotImplementedException();
       }

       public override void DeleteById(string id)
       {
           throw new NotImplementedException();
       }

       public override void Update(Survey newValue)
       {
           throw new NotImplementedException();
       }
   }
}