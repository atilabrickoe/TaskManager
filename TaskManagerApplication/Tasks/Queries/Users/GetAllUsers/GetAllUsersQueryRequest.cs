using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Commands.Users.CreateRandomUsers;

namespace TaskManagerApplication.Tasks.Queries.Users.GetAllUsers
{
    public class GetAllUsersQueryRequest : IRequest<GetAllUsersQueryResponse>
    {
        public bool WithTask { get; set; }
    }
}
