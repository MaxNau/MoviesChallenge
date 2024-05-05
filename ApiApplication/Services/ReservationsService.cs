using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Exceptions;
using ApiApplication.Services.Abstractions;
using FluentValidation;
using MapsterMapper;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class ReservationsService : IReservationsService
    {
        private readonly IMapper _mapper;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly IShowtimesRepository _showtimesRepository;
        private readonly IAuditoriumsRepository _auditoriumsRepository;
        private readonly IValidator<CreateReservationRequest> _createReservationRequestValidator;

        public ReservationsService(
            IMapper mapper,
            IReservationsRepository reservationsRepository,
            IShowtimesRepository showtimesRepository,
            IAuditoriumsRepository auditoriumsRepository,
            IValidator<CreateReservationRequest> createReservationRequestValidator)
        {
            _mapper = mapper;
            _reservationsRepository = reservationsRepository;
            _showtimesRepository = showtimesRepository;
            _auditoriumsRepository = auditoriumsRepository;
            _createReservationRequestValidator = createReservationRequestValidator;
        }

        public async Task<ReservationResponse> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var reservation = await _reservationsRepository.GetAsync(id, cancellationToken);
            return _mapper.Map<ReservationResponse>(reservation);
        }

        public async Task<ReservationResponse> CreateAsync(CreateReservationRequest reservation, CancellationToken cancelationToken)
        {
            _createReservationRequestValidator.ValidateAndThrow(reservation);

            var showtime = await _showtimesRepository.GetWithMoviesByIdAsync(reservation.ShowtimeId, cancelationToken);

            if (showtime == null)
            {
                throw new ValidationException($"Showtime - {reservation.ShowtimeId} not found");
            }

            await CheckAndThrowIfSeatsUnavaliableAsync(reservation.Seats, showtime, cancelationToken);

            var reservedSeats = _mapper.Map<List<SeatReservationEntity>>(reservation.Seats);
            reservedSeats.ForEach(s => s.AuditoriumId = showtime.AuditoriumId);

            var createdReservation = await _reservationsRepository.CreateAsync(showtime, reservedSeats, cancelationToken);
            var response = _mapper.Map<ReservationResponse>(createdReservation);
            return response;
        }

        private async Task CheckAndThrowIfSeatsUnavaliableAsync(List<Seat> seats, ShowtimeEntity showtime, CancellationToken cancelationToken)
        {
            var unavaliableSeats = new List<Seat>();
            foreach (var seat in seats)
            {
                if (!await _auditoriumsRepository.IsSeatExistsAsync(showtime.AuditoriumId, seat.Row, seat.SeatNumber, cancelationToken))
                {
                    throw new SeatReservationUnavaliableException("You are trying to reserve a seat that doesn't exist in the Auditorium");
                }

                if (!await _reservationsRepository.IsSeatAvaliableForReservationAsync(
                    showtime.Id,
                    showtime.AuditoriumId,
                    seat.Row,
                    seat.SeatNumber,
                    cancelationToken))
                {
                    unavaliableSeats.Add(seat);
                }
            }

            if (unavaliableSeats.Count > 0)
            {
                throw new SeatReservationUnavaliableException(unavaliableSeats);
            }
        }
    }
}
