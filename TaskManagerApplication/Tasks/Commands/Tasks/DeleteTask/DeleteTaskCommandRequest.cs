using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Commands.Tasks.CreateTask;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.DeleteTask
{
    public class DeleteTaskCommandRequest : IRequest<DeleteTaskCommandResponse>
    {
        public Guid TaskId { get; set; }
    }
}
