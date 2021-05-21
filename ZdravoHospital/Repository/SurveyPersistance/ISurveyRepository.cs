using Model;
using System;

namespace Repository.SurveyPersistance
{
   public interface ISurveyRepository : IRepository<string, Survey>
   {
   }
}