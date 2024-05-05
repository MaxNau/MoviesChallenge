using ApiApplication.Contracts;
using ApiApplication.Filters.Exception;
using ApiApplication.Services.Abstractions;
using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Controllers.v1
{
    [ServiceFilter(typeof(ValidationExceptionFilterAttribute))]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json", new string[] { })]
    public class ShowtimesController : ControllerBase
    {
        private readonly IShowtimesService _showtimesService;
        public ShowtimesController(IShowtimesService showtimesService)
        {
            _showtimesService = showtimesService;
        }

        [HttpGet("id")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<Showtime>> GetAsync(int id, CancellationToken cancellationToken)
        {
            return await _showtimesService.GetByIdAsync(id, cancellationToken);
        }

        [HttpPost]
        public async Task<ActionResult<Showtime>> CreateAsync([FromBody] CreateShowtimeRequest showtime, CancellationToken cancelationToken)
        {
            var result = await _showtimesService.CreateAsync(showtime, cancelationToken);

            return CreatedAtAction(nameof(GetAsync), new { id = result?.Id }, result);
        }
    }
}
