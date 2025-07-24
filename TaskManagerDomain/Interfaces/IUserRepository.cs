using TaskManagerDomain.Entities;

namespace TaskManagerDomain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User user);
        Task<User?> GetByIdAsync(Guid id);
        Task<User?> GetByIdWithTaskAsync(Guid id);
        Task<IEnumerable<User>> GetAllAsync();
        Task<IEnumerable<User>> GetAllWithTaskAsync();
        Task<User?> GetByUsernameAsync(string username);
        Task<bool> DeleteUserByIdAsync(Guid id);
    }
}
