namespace ubereats.Models.DAL.Interfaces
{
    public interface IUserRepository
    {
        Task<User> GetAsync(int? id);
        Task AddAsync(User user);
        Task RemoveAsync(User user);
        Task UpdateAsync(User user);
        Task<bool> IaExist(string login, string password);
    }
}
