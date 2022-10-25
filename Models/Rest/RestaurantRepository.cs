using Microsoft.EntityFrameworkCore;
using ubereats.Models.Context;

namespace ubereats.Models.Rest
{
    public class RestaurantRepository : IRestaurantRepository
    {
        private readonly RestaurantContext context;
        public RestaurantRepository(RestaurantContext _context)
        {
            context = _context;
        }
        // What is?
        public async Task<IEnumerable<Restaurant>> Select()
        {
            return await context.Restaurants.ToListAsync();
        }
        public async Task<Restaurant> GetAsync(int? id)
        {
#pragma warning disable CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
            return await context.Restaurants.FirstOrDefaultAsync(r => r.ID == id);
#pragma warning restore CS8603 // Возможно, возврат ссылки, допускающей значение NULL.
        }
        public async Task<List<Restaurant>> GetAllAsync()
        {
            return await context.Restaurants.ToListAsync();
        }
        public async Task<List<Restaurant>> GetForSearch(string restName)
        {
            if (!string.IsNullOrEmpty(restName))
                return await context.Restaurants.Where(r => r.RestName.ToLower().Contains(restName.ToLower())
                || r.KitchenType.ToLower().Contains(restName.ToLower())).ToListAsync();
            else
                return await context.Restaurants.ToListAsync();
        }
        public async Task<byte[]> GetImageByteById(int ID)
        {
#pragma warning disable CS8602 // Разыменование вероятной пустой ссылки.
            var img = (await context.Restaurants.FirstOrDefaultAsync(r => r.ID == ID).ConfigureAwait(false)).Image;
#pragma warning restore CS8602 // Разыменование вероятной пустой ссылки.
            //return await context.Restaurants.FirstOrDefaultAsync(r => r.ID == ID).Result.Image as Task<byte[]>;
            return img;
        }
        public async Task<Restaurant> InsertAsync(Restaurant rest)
        {
            await context.Restaurants.AddAsync(rest);
            await context.SaveChangesAsync();
            return rest;
        }
        public async Task DeleteAsync(Restaurant rest)
        {
            context.Restaurants.Remove(rest);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(Restaurant rest)
        {
            context.Restaurants.Update(rest);
            await context.SaveChangesAsync();
        }
    }
}
