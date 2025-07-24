using Microsoft.EntityFrameworkCore;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Interfaces;

namespace Repository.DataManagers.Users
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskManagerDbContext _context;

        public UserRepository(TaskManagerDbContext context) => _context = context;

        public async Task<User> CreateAsync(User user)
        {
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<IEnumerable<User>> GetAllAsync()
            => await _context.Users.ToListAsync();

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.Users.FindAsync(id);

        public async Task<User?> GetByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
    }
}