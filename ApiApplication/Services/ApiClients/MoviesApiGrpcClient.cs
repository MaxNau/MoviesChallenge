using ApiApplication.Extension;
using ProtoDefinitions;
using System.Threading.Tasks;

namespace ApiApplication.Services.ApiClients
{
    public class MoviesApiGrpcClient : MoviesApiClientBase
    {
        private readonly MoviesApi.MoviesApiClient _moviesApiClient;
        public MoviesApiGrpcClient(MoviesApi.MoviesApiClient moviesApiClient)
        {
            _moviesApiClient = moviesApiClient;
        }

        public override async Task<showListResponse> GetAllAsync()
        {
            var response = await _moviesApiClient.GetAllAsync(new Empty());
            return response.SafeUnpack<showListResponse>();
        }

        public override async Task<showResponse> GetByIdAsync(string id)
        {
            var response = await _moviesApiClient.GetByIdAsync(new IdRequest { Id = id});
            return response.SafeUnpack<showResponse>();
        }

        public override async Task<showListResponse> SearchAsync(string title)
        {
            var response = await _moviesApiClient.SearchAsync(new SearchRequest { Text = title });
            return response.SafeUnpack<showListResponse>();
        }
    }
}
