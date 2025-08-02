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
                throw new UserIsRequiredForTaskException("O usuário é necessário para a tarefa."); 
            }
            if (this.User != null)
                this.User.IsValid();

            if (string.IsNullOrWhiteSpace(Title))
            {
                throw new WrongRequiredInformation("O título não pode ser nulo ou vazio.");
            }
            if(DueDate.Date < DateTime.Now.Date)
            {
                throw new WrongRequiredInformation("A data de vencimento não pode estar no passado.");
            }
        }

        private void CanAssociate()
        {
            if(this.User != null)
            {
                throw new TaskAlreadyAssociatedException("Esta tarefa já está associada a um usuário.");
            }
        }
    }
}
