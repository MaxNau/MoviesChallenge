using ProtoDefinitions;
using System.Threading.Tasks;

namespace ApiApplication.Services.Abstractions
{
    public interface IMoviesApiClient
    {
        Task<showListResponse> GetAllAsync();
        Task<showResponse> GetByIdAsync(string id);
        Task<showListResponse> SearchAsync(string title);
    }
}
