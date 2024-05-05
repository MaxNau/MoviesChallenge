using ApiApplication.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services.Abstractions
{
    public interface ITicketsService
    {
        Task<ConfirmReservationResponse> ConfirmAsync(Guid reservationId, CancellationToken cancellationToken);
    }
}
