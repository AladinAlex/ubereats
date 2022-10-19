using Microsoft.EntityFrameworkCore;
using ubereats.DAL.Context;

namespace ubereats.Models.Authentication.User
{
    public class UserRepository : IUserRepository
    {
        private readonly RestaurantContext context;
        public UserRepository(RestaurantContext _context)
        {
            context = _context;
        }
        public async Task<User> GetAsync(int? id)
        {
            return await context.Users.FirstOrDefaultAsync(r => r.ID == id);
        }
        public async Task<User> GetAsync(User user)
        {
            return await context.Users.FirstOrDefaultAsync(r => r.loginname == user.loginname && r.password == user.password);
        }
        public async Task AddAsync(User user)
        {
            await context.Users.AddAsync(user);
            await context.SaveChangesAsync();
        }
        public async Task RemoveAsync(User user)
        {
            context.Users.Remove(user);
            await context.SaveChangesAsync();
        }
        public async Task UpdateAsync(User user)
        {
            context.Update(user);
            await context.SaveChangesAsync();
        }
        public async Task<bool> IaExist(string login, string password)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.loginname == login && u.password == password);
            if (user == null)
                return false;
            else
                return true;
        }
    }
}
