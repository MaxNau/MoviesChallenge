using ApiApplication.Database.Entities;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Database.Repositories.Abstractions
{
    public interface IAuditoriumsRepository
    {
        Task<AuditoriumEntity> GetAsync(int auditoriumId, CancellationToken cancel);
        Task<bool> ExistsAsync(int auditoriumId, CancellationToken cancel);
        Task<bool> IsSeatExistsAsync(int auditoriumId, short row, short seatNumber, CancellationToken cancel);
    }
}