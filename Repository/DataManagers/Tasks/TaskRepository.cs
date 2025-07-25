using Microsoft.EntityFrameworkCore;
using TaskManagerDomain.Entities;
using TaskManagerDomain.Interfaces;

namespace Repository.DataManagers.Tasks
{
    public class TaskRepository : ITaskRepository
    {
        private readonly TaskManagerDbContext _context;

        public TaskRepository(TaskManagerDbContext context) => _context = context;

        public async Task<TaskItem> CreateAsync(TaskItem task)
        {
            _context.TaskItens.Add(task);
            await _context.SaveChangesAsync();
            return task;
        }

        public async Task DeleteAsync(Guid id)
        {
            var task = await _context.TaskItens.FindAsync(id);
            if (task != null)
            {
                _context.TaskItens.Remove(task);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<TaskItem>> GetAllAsync()
            => await _context.TaskItens.ToListAsync();

        public async Task<TaskItem?> GetByIdAsync(Guid id)
            => await _context.TaskItens.FindAsync(id);
        public async Task<TaskItem?> GetByTitleAsync(string title)
            => await _context.TaskItens.FirstOrDefaultAsync(t => t.Title == title);

        public async Task<TaskItem> UpdateAsync(TaskItem task)
        {
            _context.TaskItens.Update(task);
            await _context.SaveChangesAsync();
            return task;
        }
    }
}
