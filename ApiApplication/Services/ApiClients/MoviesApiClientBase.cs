using ApiApplication.Services.Abstractions;
using ProtoDefinitions;
using System.Threading.Tasks;

namespace ApiApplication.Services.ApiClients
{
    public abstract class MoviesApiClientBase : IMoviesApiClient
    {
        public abstract Task<showListResponse> GetAllAsync();
        public abstract Task<showResponse> GetByIdAsync(string id);
        public abstract Task<showListResponse> SearchAsync(string title);
    }
}
