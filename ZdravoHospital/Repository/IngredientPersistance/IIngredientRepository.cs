using Model;
using Repository;
using System;
using System.Collections.Generic;
using System.Text;

namespace ZdravoHospital.Repository.IngredientPersistance
{
    interface IIngredientRepository : IRepository<string, Ingredient>
    {
    }
}
