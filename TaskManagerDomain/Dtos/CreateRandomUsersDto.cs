using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerDomain.Dtos
{
    public class CreateRandomUsersDto
    {
        public int Amount { get; set; }
        public string UserNameMask { get; set; } = string.Empty;
    }
}
