using ApiApplication.Contracts;
using ApiApplication.Database.Entities;
using Mapster;
using ProtoDefinitions;
using System;

namespace ApiApplication.Configuration
{
    public class MappingConfiguration : IRegister
    {
        public void Register(TypeAdapterConfig config)
        {
            config.NewConfig<MovieEntity, Movie>()
                .TwoWays();
            config.NewConfig<ShowtimeEntity, CreateShowtimeRequest>()
                .TwoWays();
            config.NewConfig<Show, MovieEntity>()
                .Ignore(dest => dest.Id)
                .Map(dest => dest.Stars, src => src.Crew)
                .Map(dest => dest.ImdbId, src => src.Id)
                .Map(dest => dest.ReleaseDate,
                    src => !string.IsNullOrWhiteSpace(src.Year) ? 
                        new DateTime(int.Parse(src.Year), 1, 1) : DateTime.MinValue);
            config.NewConfig<SeatEntity, Seat>()
                .TwoWays();
            config.NewConfig<SeatReservationEntity, Seat>()
                .TwoWays();

            config.NewConfig<ReservationEntity, ReservationResponse>()
                .Map(dest => dest.Movie, src => src.Showtime == null ? default : src.Showtime.Movie)
                .Map(dest => dest.AuditoriumId, src => src.Showtime == null ? default : src.Showtime.AuditoriumId)
                .Map(dest => dest.Seats, src => src.ReservedSeats);

            config.NewConfig<TicketEntity, Ticket>()
                .Map(dest => dest.Seat, src => src.TicketSeat)
                .Map(dest => dest.Showtime, src => src.Showtime)
                .TwoWays();

            ApiContractsMapping(config);
        }

        private void ApiContractsMapping(TypeAdapterConfig config)
        {
            config.NewConfig<showResponse, Show>();
        }
    }
}
