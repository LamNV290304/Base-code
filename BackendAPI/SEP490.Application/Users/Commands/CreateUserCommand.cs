using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static SEP490.Application.Interfaces.IRepository;

namespace SEP490.Application.Users.Commands
{
    public record CreateUserCommand(string FullName, string Email) : IRequest<ErrorOr<UserDto>>;

    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ErrorOr<UserDto>>
    {
        private readonly IRepository<User> _repository;
        public CreateUserCommandHandler(IRepository<User> repository) => _repository = repository;

        public async Task<ErrorOr<UserDto>> Handle(CreateUserCommand request, CancellationToken ct)
        {
            var user = new User { FullName = request.FullName, Email = request.Email };
            await _repository.AddAsync(user, ct);
            return user.ToDto();
        }
    }
}
