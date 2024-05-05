using ApiApplication.Contracts;
using ApiApplication.Services.Abstractions;
using MapsterMapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class MoviesApiService : IMoviesApiService
    {
        private readonly IMapper _mapper;
        private readonly IMoviesApiClient _moviesApiClient;
        public MoviesApiService(IMapper mapper, IMoviesApiClient moviesApiClient)
        {
            _mapper = mapper;
            _moviesApiClient = moviesApiClient;
        }

        public async Task<List<Show>> GetAllAsync()
        {
            var response = await _moviesApiClient.GetAllAsync();
            return _mapper.Map<List<Show>>(response);
        }

        public async Task<Show> GetByIdAsync(string id)
        {
            var response = await _moviesApiClient.GetByIdAsync(id);
            return _mapper.Map<Show>(response);
        }

        public async Task<List<Show>> SearchAsync(string title)
        {
            var response = await _moviesApiClient.SearchAsync(title);
            return _mapper.Map<List<Show>>(response);
        }
    }
}
