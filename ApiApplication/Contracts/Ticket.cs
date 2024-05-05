using System;

namespace ApiApplication.Contracts
{
    public class Ticket
    {
        public Guid Id { get; set; }
        public Seat Seat { get; set; }
        public DateTime CreatedTime { get; set; }
        public Showtime Showtime { get; set; }
    }
}
