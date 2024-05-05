using ApiApplication.Contracts;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services.Abstractions
{
    public interface IReservationsService
    {
        Task<ReservationResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<ReservationResponse> CreateAsync(CreateReservationRequest reservation, CancellationToken cancelationToken);
    }
}
