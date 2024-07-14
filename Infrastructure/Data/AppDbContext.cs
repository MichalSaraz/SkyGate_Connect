using Core.BaggageContext;
using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Core.PassengerContext.JoinClasses;
using Core.SeatingContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Reflection;
using Core.PassengerContext.APIS;
using Core.FlightContext.JoinClasses;
using Core.HistoryTracking;
using Infrastructure.Data.Config;

namespace Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<ScheduledFlight> ScheduledFlights { get; set; }
        public DbSet<BaseFlight> Flights { get; set; }
        public DbSet<BasePassengerOrItem> Passengers { get; set; }
        public DbSet<PassengerFlight> PassengerFlight { get; set; }
        public DbSet<PassengerBookingDetails> PassengerBookingDetails { get; set; }
        public DbSet<Baggage> Baggage { get; set; }
        public DbSet<Aircraft> Aircrafts { get; set; }
        public DbSet<Seat> Seats { get; set; }
        public DbSet<SpecialServiceRequest> SpecialServiceRequests { get; set; }
        public DbSet<AircraftType> AircraftTypes { get; set; }
        public DbSet<Airline> Airlines { get; set; }
        public DbSet<Destination> Destinations { get; set; }
        public DbSet<BookingReference> BookingReferences { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<FlightComment> FlightComment { get; set; }
        public DbSet<FlightBaggage> FlightBaggage { get; set; }
        public DbSet<PredefinedComment> PredefinedComments { get; set; }
        public DbSet<FrequentFlyer> FrequentFlyerCards { get; set; }
        public DbSet<SSRCode> SSRCodes { get; set; }
        public DbSet<APISData> APISData { get; set; }
        public DbSet<Country> Countries { get; set; }
        public DbSet<SeatMap> SeatMaps { get; set; }
        public DbSet<ActionHistory<object>> ActionHistory { get; set; }

        private readonly IConfiguration _configuration;

        public AppDbContext()
        {
        }

        public AppDbContext(DbContextOptions options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseNpgsql(_configuration.GetConnectionString("DefaultConnection"));
        } 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

            modelBuilder.HasSequence<int>("BaggageTagsSequence").StartsAt(1).IncrementsBy(1).HasMax(999999).IsCyclic();
        }
    }    
}
