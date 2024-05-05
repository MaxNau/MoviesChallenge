using System;

namespace ApiApplication.Contracts
{
    public class Showtime
    {
        public int Id { get; set; }
        public Movie Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}
