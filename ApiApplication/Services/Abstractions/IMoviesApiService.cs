using ApiApplication.Contracts;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Services.Abstractions
{
    public interface IMoviesApiService
    {
        Task<List<Show>> GetAllAsync();
        Task<Show> GetByIdAsync(string id);
        Task<List<Show>> SearchAsync(string title);
    }
}
