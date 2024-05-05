using ApiApplication.Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace ApiApplication.Database
{
    public class CinemaContext : DbContext
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options)
        {
            
        }

        public DbSet<AuditoriumEntity> Auditoriums { get; set; }
        public DbSet<ShowtimeEntity> Showtimes { get; set; }
        public DbSet<MovieEntity> Movies { get; set; }
        public DbSet<TicketEntity> Tickets { get; set; }
        public DbSet<SeatEntity> Seats { get; set; }
        public DbSet<SeatReservationEntity> SeatReservations { get; set; }
        public DbSet<ReservationEntity> Reservations { get; set; }
        public DbSet<TicketSeatEntity> TicketSeats { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AuditoriumEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.HasMany(entry => entry.Showtimes).WithOne().HasForeignKey(entity => entity.AuditoriumId);
            });

            modelBuilder.Entity<SeatEntity>(build =>
            {
                build.HasKey(entry => new { entry.AuditoriumId, entry.Row, entry.SeatNumber });
                build.HasOne(entry => entry.Auditorium).WithMany(entry => entry.Seats).HasForeignKey(entry => entry.AuditoriumId);
            });

            modelBuilder.Entity<ShowtimeEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.HasOne(entry => entry.Movie).WithMany(entry => entry.Showtimes).HasForeignKey(entry => entry.MovieId);
                build.HasMany(entry => entry.Tickets).WithOne(entry => entry.Showtime).HasForeignKey(entry => entry.ShowtimeId);
            });

            modelBuilder.Entity<MovieEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
            });

            modelBuilder.Entity<TicketSeatEntity>(build =>
            {
                build.HasKey(entry => new { entry.TicketId, entry.AuditoriumId, entry.Row, entry.SeatNumber });
                build.HasOne(entry => entry.Ticket).WithOne(entry => entry.TicketSeat).HasForeignKey<TicketSeatEntity>(entry => new { entry.TicketId, entry.AuditoriumId, entry.Row, entry.SeatNumber });

            });

            modelBuilder.Entity<TicketEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.HasOne(entry => entry.TicketSeat).WithMany();
            });

            modelBuilder.Entity<SeatReservationEntity>(build =>
            {
                build.HasKey(entry => new { entry.ReservationId, entry.AuditoriumId, entry.Row, entry.SeatNumber });
                build.HasOne(entry => entry.Reservation).WithMany(x => x.ReservedSeats).HasForeignKey(e => e.ReservationId);
            });

            modelBuilder.Entity<ReservationEntity>(build =>
            {
                build.HasKey(entry => entry.Id);
                build.Property(entry => entry.Id).ValueGeneratedOnAdd();
                build.HasOne(entry => entry.Showtime).WithMany();
                build.HasMany(x => x.ReservedSeats).WithOne(x => x.Reservation).HasForeignKey(e => e.ReservationId);
            });
        }
    }
}
