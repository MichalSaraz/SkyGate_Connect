using Core.BaggageContext;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.PassengerContext.Regulatory;
using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ScheduledFlight> ScheduledFlights { get; set; }
        public DbSet<BaseFlight> Flights { get; set; }
        public DbSet<Passenger> Passengers { get; set; }
        public DbSet<PassengerFlight> PassengerFlight { get; set; }
        public DbSet<PassengerInfo> PassengerInfo { get; set; }
        public DbSet<Baggage> Baggage { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SpecialServiceRequest> SpecialServiceRequests { get; set; }
        public DbSet<AircraftType> AircraftTypes { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<BookingReference> BookingReferences { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<PredefinedComment> PredefinedComments { get; set; }
        public DbSet<FrequentFlyer> FrequentFlyers { get; set; }
        public DbSet<SSRCode> SSRCodes { get; set; }
        public DbSet<APISData> APISData { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<SeatMap> SeatMaps { get; set; }


        protected readonly IConfiguration _configuration;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"), options =>
            {
                options.CommandTimeout(1000);
            });
            options.EnableSensitiveDataLogging();
        }        

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.HasSequence<int>("BaggageTagsSequence")
                .StartsAt(1)
                .IncrementsBy(1)
                .HasMax(999999)
                .IsCyclic();
        }
    }    
}
