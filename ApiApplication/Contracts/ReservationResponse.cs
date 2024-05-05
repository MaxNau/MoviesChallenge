using System;
using System.Collections.Generic;

namespace ApiApplication.Contracts
{
    public class ReservationResponse
    {
        public Guid Id { get; set; }
        public List<Seat> Seats { get; set; }
        public int AuditoriumId { get; set; }
        public Movie Movie { get; set; }
    }
}
