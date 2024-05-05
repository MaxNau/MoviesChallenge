using System;

namespace ApiApplication.Database.Entities
{
    public class TicketSeatEntity
    {
        public int Id { get; set; }
        public short Row { get; set; }
        public short SeatNumber { get; set; }
        public int AuditoriumId { get; set; }
        public Guid TicketId { get; set; }
        public TicketEntity Ticket { get; set; }
    }
}
