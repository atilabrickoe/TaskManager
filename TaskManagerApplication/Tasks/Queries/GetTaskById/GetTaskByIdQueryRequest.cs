using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Users.Queries.GetById;

namespace TaskManagerApplication.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryRequest : IRequest<GetTaskByIdQueryResponse>
    {
        public Guid Id { get; set; }
        public bool WithUser { get; set; }
    }
}
