using ErrorOr;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SEP490.API.Requests;
using SEP490.Application.Users.Commands;
using SEP490.Application.Users.Queries;

namespace SEP490.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly ISender _mediator;
        public UsersController(ISender mediator) => _mediator = mediator;

        [HttpGet]
        public async Task<IActionResult> GetUsers(
            [FromQuery] string? searchTerm,
            [FromQuery] int pageIndex = 1,
            [FromQuery] int pageSize = 10,
            CancellationToken ct = default)
        {
            // Tạo query từ các tham số nhận được
            var query = new GetUsersQuery(searchTerm, pageIndex, pageSize);

            // Gửi query đến Handler xử lý thông qua MediatR
            var result = await _mediator.Send(query, ct);

            // Xử lý kết quả trả về từ ErrorOr
            return result.Match(
                response => Ok(response), // Trả về 200 OK kèm theo UserPagedResponse (Items và TotalCount)
                errors => Problem(errors.First().Description) // Xử lý lỗi nếu có
            );
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetUserQuery(id));
            return result.Match(
                Ok,
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => NotFound(errors.First().Description),
                    ErrorType.Validation => BadRequest(errors),
                    ErrorType.Conflict => Conflict(errors.First().Description),
                    _ => Problem(errors.First().Description)
                }
            );
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Match(
                Ok,
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => NotFound(errors.First().Description),
                    ErrorType.Validation => BadRequest(errors),
                    ErrorType.Conflict => Conflict(errors.First().Description),
                    _ => Problem(errors.First().Description)
                }
            );
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, request.FullName, request.Email));
            return result.Match(
                Ok,
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => NotFound(errors.First().Description),
                    ErrorType.Validation => BadRequest(errors),
                    ErrorType.Conflict => Conflict(errors.First().Description),
                    _ => Problem(errors.First().Description)
                }
            );
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result.Match<IActionResult>(
                _ => NoContent(),
                errors => errors.First().Type switch
                {
                    ErrorType.NotFound => NotFound(errors.First().Description),
                    ErrorType.Validation => BadRequest(errors),
                    ErrorType.Conflict => Conflict(errors.First().Description),
                    _ => Problem(errors.First().Description)
                }
            );
        }
    }
}
