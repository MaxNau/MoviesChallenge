using System;
using System.Collections.Generic;

namespace ApiApplication.Database.Entities
{
    public class ReservationEntity
    {
        public ReservationEntity()
        {
            Created = DateTime.UtcNow;
        }

        public Guid Id { get; set; }
        public int ShowtimeId { get; set; }
        public ReservationStatus Status { get; set; }
        public DateTime Created { get; set; }
        public ShowtimeEntity Showtime { get; set; }
        public ICollection<SeatReservationEntity> ReservedSeats { get; set; }
    }
}
