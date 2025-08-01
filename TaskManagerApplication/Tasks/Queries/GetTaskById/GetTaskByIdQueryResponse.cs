using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Queries.GetTaskById
{
    public class GetTaskByIdQueryResponse : Response
    {
        public TaskDto Data { get; set; }
    }
}
