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

        public static TaskDto MapToDto(TaskItem task)
        {
            return new TaskDto()
            {
                Id = task.Id,
                Title = task.Title,
                Description = task.Description,
                Status = task.Status,
            };
        }
    }
}
