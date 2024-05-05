using ApiApplication.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Exceptions
{
    public class SeatReservationUnavaliableException : Exception
    {
        public SeatReservationUnavaliableException(List<Seat> seats)
            : base($"Some of the seats are unavaliable: { string.Join(", ", seats?.Select(s => $"{s.Row}:{s.SeatNumber}")) }")
        {
        }

        public SeatReservationUnavaliableException(string message)
            : base(message)
        {
        }
    }
}
