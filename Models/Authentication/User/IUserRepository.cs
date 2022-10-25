namespace ubereats.Models.Authentication.User
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int? id);
        Task<User> GetAsync(User user);
        Task AddAsync(User user);
        Task RemoveAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IaExist(string login, string password);
    }
}
