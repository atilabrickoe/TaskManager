using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Queries.Users.GetAllUsers;

namespace TaskManagerApplication.Tasks.Queries.Tasks
{
    public class GetAllTasksQueryRequest : IRequest<GetAllTasksQueryResponse>
    {
        public bool WithUser { get; set; }
    }
}
