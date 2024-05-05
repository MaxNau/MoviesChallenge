using System;
using System.Collections.Generic;

namespace ApiApplication.Contracts
{
    public class ConfirmReservationResponse
    {
        public Guid ReservationId { get; set; }
        public List<Ticket> Tickets { get; set; }
    }
}
