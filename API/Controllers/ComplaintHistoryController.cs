using Application.Features.Commands.ComplaintHistoryCommands;
using Application.Features.Queries.ComplaintHistoryQueries;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintHistoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComplaintHistoryController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllComplaintHistoriesQuery());
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetComplaintHistoryByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComplaintHistoryCommand command)
        {
            var id = await _mediator.Send(command);
            var createdItem = await _mediator.Send(new GetComplaintHistoryByIdQuery(id));
            return CreatedAtAction(nameof(GetById), new { id }, createdItem);
        }

      

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteComplaintHistoryCommand { Id = id });
            return NoContent();
        }
    }
}
