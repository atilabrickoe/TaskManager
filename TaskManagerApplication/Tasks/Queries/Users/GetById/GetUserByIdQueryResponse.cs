using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Queries.Users.GetById
{
    public class GetUserByIdQueryResponse : Response
    {
        public UserDto User { get; set; }
    }
}
