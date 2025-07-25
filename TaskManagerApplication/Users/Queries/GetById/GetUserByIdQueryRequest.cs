using MediatR;

namespace TaskManagerApplication.Users.Queries.GetById
{
    public class GetUserByIdQueryRequest : IRequest<GetUserByIdQueryResponse>
    {
        public Guid Id { get; set; }
        public bool WithTask { get; set; }
    }
}
