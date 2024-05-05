using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;
using ApiApplication.Database.Repositories.Abstractions;

namespace ApiApplication.Database.Repositories
{
    public class AuditoriumsRepository : IAuditoriumsRepository
    {
        private readonly CinemaContext _context;

        public AuditoriumsRepository(CinemaContext context)
        {
            _context = context;
        }

        public async Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums
                .Include(x => x.Seats)
                .FirstOrDefaultAsync(x => x.Id == auditoriumId, cancel);
        }

        public async Task<bool> ExistsAsync(int auditoriumId, CancellationToken cancel)
        {
            return await _context.Auditoriums.AnyAsync(x => x.Id == auditoriumId, cancel);
        }

        public async Task<bool> IsSeatExistsAsync(int auditoriumId, short row, short seatNumber, CancellationToken cancel)
        {
            return await _context.Seats.AnyAsync(x => x.AuditoriumId == auditoriumId &&
                x.Row == row && x.SeatNumber == seatNumber, cancel);
        }
    }
}
