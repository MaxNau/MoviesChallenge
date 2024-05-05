using System;

namespace ApiApplication.Contracts
{
    public class CreateShowtimeRequest
    {
        public MovieRequest Movie { get; set; }
        public DateTime SessionDate { get; set; }
        public int AuditoriumId { get; set; }
    }
}
