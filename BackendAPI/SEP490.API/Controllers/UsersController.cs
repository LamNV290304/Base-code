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

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var result = await _mediator.Send(new GetUserQuery(id));
            return result.Match(Ok, err => Problem(err.First().Description));
        }

        [HttpPost]
        public async Task<IActionResult> Create(CreateUserCommand command)
        {
            var result = await _mediator.Send(command);
            return result.Match(res => CreatedAtAction(nameof(Get), new { id = res.Id }, res),
                                err => Problem(err.First().Description));
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, UpdateUserRequest request)
        {
            var result = await _mediator.Send(new UpdateUserCommand(id, request.FullName, request.Email));
            return result.Match(Ok, err => Problem(err.First().Description));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _mediator.Send(new DeleteUserCommand(id));
            return result.Match<IActionResult>(_ => NoContent(), err => Problem(err.First().Description));
        }
    }
}
