using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Users.Queries.GetById
{
    public class GetUserByIdQueryResponse : Response
    {
        public UserDto Data { get; set; }
    }
}
