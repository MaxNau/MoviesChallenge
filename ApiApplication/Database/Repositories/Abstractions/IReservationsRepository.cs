using ApiApplication.Database.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IReservationsRepository
    {
        Task<ReservationEntity> CreateAsync(ShowtimeEntity showtime,
            List<SeatReservationEntity> selectedSeats, CancellationToken cancel);
        Task<ReservationEntity> GetAsync(Guid id, CancellationToken cancel);
        Task<bool> IsSeatAvaliableForReservationAsync(int showtimeId, int auditoriumId, short row, short seatNumber, CancellationToken cancellationToken);
        Task<ReservationEntity> ConfirmReservationAsync(ReservationEntity reservationEntity, CancellationToken cancellationToken);
    }
}
