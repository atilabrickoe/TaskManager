using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerDomain.Exceptions
{
    public class WrongRequiredInformation : Exception
    {
        public WrongRequiredInformation(string message) : base(message) 
        {
        }
    }
}
