using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Application.Users.Queries
{
    public record UserPagedResponse(IEnumerable<UserDto> Items, int TotalCount);

    public record GetUsersQuery(string? SearchTerm, int PageIndex, int PageSize)
        : IRequest<ErrorOr<UserPagedResponse>>;

    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, ErrorOr<UserPagedResponse>>
    {
        private readonly IRepository<User> _repository;

        public GetUsersQueryHandler(IRepository<User> repository)
            => _repository = repository;

        public async Task<ErrorOr<UserPagedResponse>> Handle(GetUsersQuery request, CancellationToken ct)
        {
            var query = _repository.Query();

            query = query.Where(u => !u.IsDeleted);

            if (!string.IsNullOrEmpty(request.SearchTerm))
            {
                query = query.Where(u => u.FullName.Contains(request.SearchTerm) ||
                                         u.Email.Contains(request.SearchTerm));
            }

            query = query.OrderByDescending(u => u.CreatedAt);

            var (items, total) = await _repository.GetPagedAsync(
                request.PageIndex,
                request.PageSize,
                query,
                ct);

            var userDtos = items.Select(u => u.ToDto()).ToList();

            return new UserPagedResponse(userDtos, total);
        }
    }
}