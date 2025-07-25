﻿using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.Tasks.CreateTask
{
    public class CreateTaskCommandResponse : Response
    {
        public TaskDto Task { get; set; }
    }
}
