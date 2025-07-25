using MediatR;

namespace TaskManagerApplication.Users.Queries.GetAllUsers
{
    public class GetAllUsersQueryRequest : IRequest<GetAllUsersQueryResponse>
    {
        public bool WithTask { get; set; }
    }
}
