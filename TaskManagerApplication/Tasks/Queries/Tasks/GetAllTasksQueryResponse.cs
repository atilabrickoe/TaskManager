using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Tasks.Queries.Tasks
{
    public class GetAllTasksQueryResponse : Response
    {
        public List<TaskDto> Task { get; set; }
    }
}
