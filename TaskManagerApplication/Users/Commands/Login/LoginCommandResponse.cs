using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaskManagerApplication.TokenJwtAuthentication;

namespace TaskManagerApplication.Users.Commands.Login
{
    public class LoginCommandResponse : Response
    {
        public TokenJwt Token { get; set; }
    }
}
