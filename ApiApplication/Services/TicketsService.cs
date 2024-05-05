using ApiApplication.Configuration;
using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
using ApiApplication.Database.Repositories.Abstractions;
using ApiApplication.Services.Abstractions;
using FluentValidation;
using MapsterMapper;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ApiApplication.Services
{
    public class TicketsService : ITicketsService
    {
        private readonly IMapper _mapper;
        private readonly ITicketsRepository _ticketsRepository;
        private readonly IReservationsRepository _reservationsRepository;
        private readonly ReservationConfig _reservationConfig;

        public TicketsService(
            IMapper mapper,
            ITicketsRepository ticketsRepository,
            IReservationsRepository reservationsRepository,
            IOptions<ReservationConfig> options)
        {
            _mapper = mapper;
            _ticketsRepository = ticketsRepository;
            _reservationsRepository = reservationsRepository;
            _reservationConfig = options.Value;
        }

        public async Task<ConfirmReservationResponse> ConfirmAsync(Guid reservationId, CancellationToken cancellationToken)
        {
            if (reservationId == null || reservationId == Guid.Empty)
            {
                throw new ValidationException("Reservation id is required");
            }

            var reservation = await _reservationsRepository.GetAsync(reservationId, cancellationToken);

            if (reservation.Created <= DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(_reservationConfig.ExpirationTimeMinutes)))
            {
                throw new ValidationException($"Reservation {reservationId} number has been expired");
            }
            
            if (reservation.Status == ReservationStatus.Paid)
            {
                throw new ValidationException($"Reservation {reservationId} has already been confirmed");
            }

            var tickets = await _ticketsRepository.CreateBulkAsync(reservation.Showtime, reservation.ReservedSeats, cancellationToken);

            foreach (var ticket in tickets)
            {
                await _ticketsRepository.ConfirmPaymentAsync(ticket, cancellationToken);
            }

            await _reservationsRepository.ConfirmReservationAsync(reservation, cancellationToken);

            return new ConfirmReservationResponse
            {
                ReservationId = reservation.Id,
                Tickets = _mapper.Map<List<Ticket>>(tickets)
            };
        }
    }
}
