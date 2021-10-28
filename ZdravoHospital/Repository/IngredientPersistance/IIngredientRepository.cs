using Model;
using Repository;

namespace ZdravoHospital.Repository.IngredientPersistance
{
    public interface IIngredientRepository : IRepository<string, Ingredient>
    {
    }
}
