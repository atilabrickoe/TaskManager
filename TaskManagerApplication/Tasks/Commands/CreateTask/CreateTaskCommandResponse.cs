﻿using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Commands.CreateTask
{
    public class CreateTaskCommandResponse : Response
    {
        public TaskDto Data { get; set; }
    }
}
