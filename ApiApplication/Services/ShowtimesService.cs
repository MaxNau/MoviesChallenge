using ApiApplication.Cache;
using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Services.Abstractions;
using FluentValidation;
using MapsterMapper;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class ShowtimesService : IShowtimesService
    {
        private readonly IMapper _mapper;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IMoviesApiService _moviesApiService;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly ICache _cache;
        private readonly IValidator<CreateShowtimeRequest> _validator;

        public ShowtimesService(
            IMapper mapper, 
            IShowtimesRepository showtimesRepository,
            IMoviesApiService moviesApiService,
            IAuditoriumsRepository auditoriumsRepository,
            ICache cache,
            IValidator<CreateShowtimeRequest> validator)
        {
            _mapper = mapper;
            _showtimesRepository = showtimesRepository;
            _moviesApiService = moviesApiService;
            _auditoriumsRepository = auditoriumsRepository;
            _cache = cache;
            _validator = validator;
        }

        public async Task<Showtime> GetByIdAsync(int id, CancellationToken cancellationToken)
        {
            var entity = await _showtimesRepository.GetWithMoviesByIdAsync(id, cancellationToken);
            return _mapper.Map<Showtime>(entity);
        }

        public async Task<Showtime> CreateAsync(CreateShowtimeRequest createShowtimeRequest, CancellationToken cancellationToken)
        {
            _validator.ValidateAndThrow(createShowtimeRequest);

            if (!await _auditoriumsRepository.ExistsAsync(createShowtimeRequest.AuditoriumId, cancellationToken))
            {
                throw new ValidationException($"The Auditorium with Id '{createShowtimeRequest.AuditoriumId}' is not found");
            }

            Show show = null;
            try
            {
                show = await _moviesApiService.GetByIdAsync(createShowtimeRequest.Movie.ImdbId);

                if (show == null)
                {
                    throw new ValidationException($"The Movie with ImdbId '{createShowtimeRequest.Movie.ImdbId}' not found");
                }

                await _cache.SetAsync($"{nameof(Show)}{createShowtimeRequest.Movie.ImdbId}", show);
            }
            catch (ValidationException)
            {
                throw;
            }
            catch (Exception)
            {
                show = await _cache.GetAsync<Show>($"{nameof(Show)}{createShowtimeRequest.Movie.ImdbId}");
                if (show == null)
                {
                    throw;
                }
            }

            var showTime = _mapper.Map<ShowtimeEntity>(createShowtimeRequest);
            showTime.Movie = _mapper.Map<MovieEntity>(show);
            var entity = await _showtimesRepository.CreateShowtimeAsync(showTime, cancellationToken);
            return _mapper.Map<Showtime>(entity);
        }
    }
}
