using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Application.Users.Queries
{
    public record GetUserQuery(int Id) : IRequest<ErrorOr<UserDto>>;
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, ErrorOr<UserDto>>
    {
        private readonly IRepository<User> _repository;
        public GetUserQueryHandler(IRepository<User> repository) => _repository = repository;

        public async Task<ErrorOr<UserDto>> Handle(GetUserQuery request, CancellationToken ct)
        {
            var user = await _repository.GetAsync(request.Id, ct);
            if (user is null || user.IsDeleted) return Error.NotFound("User.NotFound");

            return user.ToDto();
        }
    }
}
