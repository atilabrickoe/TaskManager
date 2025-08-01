using TaskManagerDomain.Dtos;
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

        public void IsValid()
        {
            if (this.User == null)
            {
                throw new UserIsRequiredForTaskException("User is required for task."); 
            }
            if (this.User != null)
                this.User.IsValid();

            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new WrongRequiredInformation("Title cannot be null or empty.");
            }
            if(DueDate < DateTime.Now)
            {
                throw new WrongRequiredInformation("Due date cannot be in the past.");
            }
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
