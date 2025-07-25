using TaskManagerDomain.Dtos;

namespace TaskManagerApplication.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryResponse : Response
    {
        public List<UserDto> Users { get; set; }
    }
}
