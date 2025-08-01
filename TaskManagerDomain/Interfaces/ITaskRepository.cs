﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Entities;

namespace TaskManagerDomain.Interfaces
{
    public interface ITaskRepository
    {
        Task<TaskItem> CreateAsync(TaskItem task);
        Task<TaskItem?> GetByIdAsync(Guid id);
        Task<TaskItem?> GetByTitleAsync(string title);
        Task<IEnumerable<TaskItem>> GetAllAsync();
        Task<IEnumerable<TaskItem>> GetAllWithUserAsync();
        Task<TaskItem> UpdateAsync(TaskItem task);
        Task DeleteAsync(Guid id);
        Task<TaskItem?> GetByIdWithUserAsync(Guid id);
        Task<bool> TaskExistsToUser(string title, Guid id);
    }
}
