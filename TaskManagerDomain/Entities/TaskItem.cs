using TaskManagerDomain.Exceptions;
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
        public User? User { get; set; }

        public void Associate(User user)
        {
            CanAssociate();
            user.IsValid();

            this.User = user;
        }

        private void CanAssociate()
        {
            if(this.User != null)
            {
                throw new TaskAlreadyAssociatedException("This task is already associated with a user.");
            }
        }
    }
}
