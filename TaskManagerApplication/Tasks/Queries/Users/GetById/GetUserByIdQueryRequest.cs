using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Queries.Users.GetAllUsers;

namespace TaskManagerApplication.Tasks.Queries.Users.GetById
{
    public class GetUserByIdQueryRequest : IRequest<GetUserByIdQueryResponse>
    {
        public Guid Id { get; set; }
        public bool WithTask { get; set; }
    }
}
