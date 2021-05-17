using System;

namespace Model.Repository
{
    public class RoomInventoryRepository : Repository<int, RoomInventory>
    {
        private static string path = @"..\..\..\Resources\roomInventory.json";

        public RoomInventoryRepository() : base(path)
        {
        }

        public override RoomInventory GetById(int id)
        {
            throw new NotImplementedException();
        }

        public override void DeleteById(int id)
        {
            throw new NotImplementedException();
        }

        public override void Update(RoomInventory newValue)
        {
            throw new NotImplementedException();
        }
    }
}