using System.Threading;
using Model;
using Model.Repository;

namespace ZdravoHospital.Repository.MedicinePersistance
{
    public interface IMedicineRepository : IRepository<Medicine, string>
    {
        protected Mutex GetMutex();
    }
}