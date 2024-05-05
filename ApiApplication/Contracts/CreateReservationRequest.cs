using System.Collections.Generic;

namespace ApiApplication.Contracts
{
    public class CreateReservationRequest
    {
        public int ShowtimeId { get; set; }
        public List<Seat> Seats { get; set; }
    }
}
