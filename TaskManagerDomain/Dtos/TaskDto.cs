using TaskManagerDomain.Entities;
using Enum = TaskManagerDomain.Enums;

namespace TaskManagerDomain.Dtos
{
    public class TaskDto
    {
        public Guid Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public Enum.TaskStatus Status { get; set; }
        public Guid IdUser { get; set; }
        public DateTime DueDate { get; set; }

        public static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
                IdUser = task.User?.Id ?? Guid.Empty,
                DueDate = task.DueDate
            };
        }

        public static TaskItem MapToTask(TaskDto taskDto)
        {
            return new TaskItem()
            {
                Id = taskDto.Id,
                Title = taskDto.Title,
                Description = taskDto.Description,
                Status = taskDto.Status,
                DueDate = taskDto.DueDate,
                User = new User() { Id = taskDto.IdUser }
            };
        }
    }
}
