using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;
using TaskManagerDomain.Entities;

namespace TaskManagerApplication.Tasks.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryResponse : Response
    {
        public List<UserDto> Users { get; set; }
    }
}
