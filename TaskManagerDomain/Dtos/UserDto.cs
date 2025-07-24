using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Entities;

namespace TaskManagerDomain.Dtos
{
    public class UserDto
    {
        public Guid Id { get; set; }
        public string UserName { get; set; } = string.Empty;
        public List<TaskDto> Tasks { get; set; }

        public static UserDto MapToDto(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }
            return new UserDto()
            {
                Id = user.Id,
                UserName = user.UserName,
                Tasks = user.Tasks == null ? new List<TaskDto>() : user.Tasks.Select(t => TaskDto.MapToDto(t)).ToList()
            };
        }
    }
}
