using ubereats.Models;

namespace ubereats.Models.Restaurant
{
    public interface IRestaurantRepository
    {
        Task<IEnumerable<Restaurant>> Select();
        Task<Restaurant> GetAsync(int? id);
        Task<List<Restaurant>> GetAllAsync();
        Task<Restaurant> InsertAsync(Restaurant rest);
    }
}
