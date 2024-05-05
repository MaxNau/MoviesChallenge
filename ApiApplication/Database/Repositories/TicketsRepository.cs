using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Linq;
using ApiApplication.Database.Repositories.Abstractions;

namespace ApiApplication.Database.Repositories
{
    public class TicketsRepository : ITicketsRepository
    {
        private readonly CinemaContext _context;

        public TicketsRepository(CinemaContext context)
        {
            _context = context;
        }

        public Task<TicketEntity> GetAsync(Guid id, CancellationToken cancel)
        {
            return _context.Tickets.FirstOrDefaultAsync(x => x.Id == id, cancel);
        }

        public async Task<IEnumerable<TicketEntity>> GetEnrichedAsync(int showtimeId, CancellationToken cancel)
        {
            return await _context.Tickets
                .Include(x => x.Showtime)
                .Include(x => x.TicketSeat)
                .Where(x => x.ShowtimeId == showtimeId)
                .ToListAsync(cancel);
        }

        public async Task<TicketEntity> CreateAsync(ShowtimeEntity showtime, SeatEntity selectedSeat, CancellationToken cancel)
        {
            var ticket = _context.Tickets.Add(new TicketEntity
            {
                Showtime = showtime,
                TicketSeat = new TicketSeatEntity
                {
                    SeatNumber = selectedSeat.SeatNumber,
                    AuditoriumId = selectedSeat.AuditoriumId,
                    Row = selectedSeat.Row
                }
            });

            await _context.SaveChangesAsync(cancel);

            return ticket.Entity;
        }

        public async Task<IEnumerable<TicketEntity>> CreateBulkAsync(ShowtimeEntity showtime, IEnumerable<SeatReservationEntity> selectedSeats, CancellationToken cancel)
        {
            if (showtime == null || !selectedSeats.Any())
            {
                return default;
            }

            var tickets = selectedSeats.Select(seat => new TicketEntity
            {
                Showtime = showtime,
                TicketSeat = new TicketSeatEntity
                {
                    SeatNumber = seat.SeatNumber,
                    AuditoriumId = seat.AuditoriumId,
                    Row = seat.Row
                }
            }).ToList();

            await _context.Tickets.AddRangeAsync(tickets);

            await _context.SaveChangesAsync(cancel);

            return tickets;
        }

        public async Task<TicketEntity> ConfirmPaymentAsync(TicketEntity ticket, CancellationToken cancel)
        {
            ticket.Paid = true;
            _context.Update(ticket);
            await _context.SaveChangesAsync(cancel);
            return ticket;
        }
    }
}
