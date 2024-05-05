using System;

namespace ApiApplication.Database.Entities
{
    public class SeatReservationEntity
    {
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public int AuditoriumId { get; set; }
        public Guid ReservationId { get; set; }
        public ReservationEntity Reservation { get; set; }
    }
}
