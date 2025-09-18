using FastFood.DB;

namespace FastFood.Repositories
{
    public abstract class CommonRepository
    {
        protected readonly FastFoodEntities _fastFoodEntities;
        protected CommonRepository(FastFoodEntities fastFoodEntities)
        {
            _fastFoodEntities = fastFoodEntities;
        }
    }
}
