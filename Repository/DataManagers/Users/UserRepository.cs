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
        public async Task<IEnumerable<User>> GetAllWithTaskAsync()
        {
            var usersWithTasks = await _context.Users
                                 .Include(u => u.Tasks)
                                 .ToListAsync();
            return usersWithTasks;
        }
            

        public async Task<User?> GetByIdAsync(Guid id)
            => await _context.Users.FindAsync(id);
        public async Task<User?> GetByIdWithTaskAsync(Guid id)
        {
            var userTask = await _context.Users
                                          .Include(u => u.Tasks)
                                          .Where(u => u.Id == id).FirstAsync();
            return userTask;
        }

        public async Task<User?> GetByUsernameAsync(string username)
            => await _context.Users.FirstOrDefaultAsync(u => u.UserName == username);
        
        public async Task<User?> LoginAsync(string username, string passWord)
        {
            var userLogin = await _context.Users.FirstOrDefaultAsync(u => u.UserName == username && u.PasswordHash == passWord);
            return userLogin;
        }
        public async Task<bool> DeleteUserByIdAsync(Guid id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
                return false;

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return true;
        }

    }
}