using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Users.Queries.GetById
{
    public class GetUserByIdQueryResponse : Response
    {
        public UserDto User { get; set; }
    }
}
