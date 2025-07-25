using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.Tasks.Commands.Tasks.CreateTask;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.UpdateTask
{
    public class UpdateTaskCommandRequest : IRequest<UpdateTaskCommandResponse>
    {
        public TaskDto Task { get; set; }
    }
}
