using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Exceptions;

namespace TaskManagerDomain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public List<TaskItem> Tasks { get; set; }

        public void IsValid()
        {
            if(string.IsNullOrWhiteSpace(UserName))
            {
                throw new WrongRequiredInformation("UserName cannot be null or empty.");
            }
            if (Guid.Empty == Id)
            {
                throw new WrongRequiredInformation("Id cannot be empty.");
            }
        }
    }
}
