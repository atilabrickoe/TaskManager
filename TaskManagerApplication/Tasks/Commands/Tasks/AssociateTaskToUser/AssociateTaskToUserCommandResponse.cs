using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.AssociateTaskToUser
{
    public class AssociateTaskToUserCommandResponse : Response
    {
        public TaskDto Task { get; set; }
    }
}
