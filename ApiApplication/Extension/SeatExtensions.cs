using ApiApplication.Contracts;
using System.Collections.Generic;
using System.Linq;

namespace ApiApplication.Extension
{
    public static class SeatExtensions
    {
        private static readonly short RowMultiplier = 100;
        public static int GetSeatIndex(this Seat seat)
        {
            return seat.Row * RowMultiplier + seat.SeatNumber;
        }

        public static bool IsSeatsContiguous(this List<Seat> seats)
        {
            if (seats == null || seats.Count == 0)
            {
                return false;
            }

            if (seats.Count == 1)
            {
                return true;
            }

            int minSeat = seats.Min(seat => seat.GetSeatIndex());
            int maxSeat = seats.Max(seat => seat.GetSeatIndex());

            int seatIndexSum = (minSeat + maxSeat) * (maxSeat - minSeat + 1) / 2;
            var expectedSeatIndexSum = seats.Sum(seat => seat.GetSeatIndex());
            return seatIndexSum == expectedSeatIndexSum;
        }
    }
}
