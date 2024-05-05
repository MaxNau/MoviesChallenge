using ApiApplication.Contracts;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Threading;
using System;
using Asp.Versioning;
using ApiApplication.Services.Abstractions;
using ApiApplication.Filters.Exception;

namespace ApiApplication.Controllers.v1
{
    [ServiceFilter(typeof(ValidationExceptionFilterAttribute))]
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/[controller]")]
    [Produces("application/json", new string[] { })]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationsService _reservationsService;
        private readonly ITicketsService _ticketsService;
        public ReservationController(
            IReservationsService reservationsService,
            ITicketsService ticketsService)
        {
            _reservationsService = reservationsService;
            _ticketsService = ticketsService;
        }

        [HttpGet("id")]
        [ActionName(nameof(GetAsync))]
        public async Task<ActionResult<ReservationResponse>> GetAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _reservationsService.GetByIdAsync(id, cancellationToken);
        }

        [HttpPost("Create")]
        public async Task<ActionResult<ReservationResponse>> CreateAsync([FromBody] CreateReservationRequest reservation, CancellationToken cancelationToken)
        {
            var response = await _reservationsService.CreateAsync(reservation, cancelationToken);
            return CreatedAtAction(nameof(GetAsync), new { id = response?.Id }, response);
        }

        [HttpPost("Confirm")]
        public async Task<ActionResult<ConfirmReservationResponse>> ConfirmAsync([FromBody] Guid id, CancellationToken cancelationToken)
        {
            return Ok(await _ticketsService.ConfirmAsync(id, cancelationToken));
        }
    }
}
