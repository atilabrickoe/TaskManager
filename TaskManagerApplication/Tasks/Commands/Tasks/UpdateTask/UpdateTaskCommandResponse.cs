using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.UpdateTask
{
    public class UpdateTaskCommandResponse : Response
    {
        public TaskDto Task { get; set; }
    }
}
