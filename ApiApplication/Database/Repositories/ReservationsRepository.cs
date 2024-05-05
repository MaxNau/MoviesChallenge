using ApiApplication.Configuration;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories
{
    public class ReservationsRepository : IReservationsRepository
    {
        private readonly ReservationConfig _reservationConfig;
        private readonly CinemaContext _context;

        public ReservationsRepository(
            IOptions<ReservationConfig> options,
            CinemaContext context)
        {
            _reservationConfig = options.Value;
            _context = context;
        }

        public async Task<ReservationEntity> CreateAsync(ShowtimeEntity showtime, 
            List<SeatReservationEntity> selectedSeats, CancellationToken cancel)
        {
            var reservation = new ReservationEntity
            {
                ShowtimeId = showtime.Id,
                ReservedSeats = selectedSeats
            };
            await _context.SeatReservations.AddRangeAsync(selectedSeats);
            var entry = _context.Reservations.Add(reservation);

            await _context.SaveChangesAsync(cancel);

            return entry.Entity;
        }

        public async Task<ReservationEntity> GetAsync(Guid id, CancellationToken cancel)
        {
            return await _context.Reservations
                .Include(x => x.ReservedSeats)
                .Include(x => x.Showtime)
                   .ThenInclude(x => x.Movie)
                .FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public async Task<bool> IsSeatAvaliableForReservationAsync(int showtimeId, int auditoriumId, short row, short seatNumber, CancellationToken cancelationToken)
        {
            var expirationThreshold = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(_reservationConfig.ExpirationTimeMinutes));
            return !await _context.SeatReservations
                .Where(x => x.Reservation.ShowtimeId == showtimeId && x.Reservation.Created >= expirationThreshold
                     || x.Reservation.Status == ReservationStatus.Paid)
                .AnyAsync(x => x.AuditoriumId == auditoriumId && x.Row == row && x.SeatNumber == seatNumber, cancelationToken);
        }

        public async Task<ReservationEntity> ConfirmReservationAsync(ReservationEntity reservationEntity, CancellationToken cancelationToken)
        {
            reservationEntity.Status = ReservationStatus.Paid;
            _context.Update(reservationEntity);
            await _context.SaveChangesAsync(cancelationToken);
            return reservationEntity;
        }
    }
}
