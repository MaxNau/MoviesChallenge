using ApiApplication.Contracts;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services.Abstractions
{
    public interface IShowtimesService
    {
        Task<Showtime> GetByIdAsync(int id, CancellationToken cancellationToken);
        Task<Showtime> CreateAsync(CreateShowtimeRequest showtime, CancellationToken cancellationToken);
    }
}
