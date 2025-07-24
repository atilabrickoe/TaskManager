using Enum = TaskManagerDomain.Enums;

namespace TaskManagerDomain.Entities
{
    public class TaskItem
    {
        public Guid Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public Enum.TaskStatus Status { get; set; } = Enum.TaskStatus.Pending;
        public Guid AssignedUserId { get; set; }
    }
}
