using Application.Features.Commands.ComplaintCommands;
using Application.Features.Queries.ComplaintQueries;
using Application.Features.Results.ComplaintResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComplaintsController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ComplaintsController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: api/Complaints
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _mediator.Send(new GetAllComplaintsQuery());
            return Ok(result);
        }

        // GET: api/Complaints/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _mediator.Send(new GetComplaintByIdQuery(id));
            if (result == null)
                return NotFound();

            return Ok(result);
        }

        // POST: api/Complaints
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateComplaintCommand command)
        {
            var id = await _mediator.Send(command);

            // Kaydı ekledikten sonra include ile tekrar çekmek için GetById query'sini çağırıyoruz
            var createdComplaint = await _mediator.Send(new GetComplaintByIdQuery(id));

            return CreatedAtAction(nameof(GetById), new { id }, createdComplaint);
        }

        // PUT: api/Complaints/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateComplaintCommand command)
        {
            if (id != command.Id)
                return BadRequest();

            await _mediator.Send(command);
            return NoContent();
        }

        // DELETE: api/Complaints/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _mediator.Send(new DeleteComplaintCommand { Id = id });
            return NoContent();
        }
    }
}
