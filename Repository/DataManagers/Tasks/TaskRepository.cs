﻿using Microsoft.EntityFrameworkCore;
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
        public async Task<IEnumerable<TaskItem>> GetAllWithUserAsync()
        {
            var taskWithUser = await _context.TaskItens
                                                       .Include(t => t.User)
                                                       .ToListAsync();
            return taskWithUser;
        }

        public async Task<TaskItem?> GetByIdAsync(Guid id)
            => await _context.TaskItens.FindAsync(id);
        public async Task<TaskItem?> GetByIdWithUserAsync(Guid id)
            => await _context.TaskItens.Include(t => t.User).FirstOrDefaultAsync(t => t.Id == id);
        public async Task<bool> TaskExistsToUser(string title, Guid id)
        {
            return await _context.TaskItens.AnyAsync(t => t.Title == title && t.User.Id == id);
        }

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
