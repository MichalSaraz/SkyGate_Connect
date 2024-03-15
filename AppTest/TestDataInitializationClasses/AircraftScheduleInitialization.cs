using Core.FlightContext;
using Core.FlightContext.FlightInfo;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
{
    public class AircraftScheduleInitialization
    {
        private readonly AppDbContext dbContext;

        public AircraftScheduleInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void PlanFlights()
        {
            var flights = dbContext.Flights.ToList();
            var aircrafts = dbContext.Aircrafts.ToList();
            var scheduledFlights = dbContext.ScheduledFlights.ToList();

            var availableFlights = new List<Flight>();

            foreach (var aircraft in aircrafts)
            {
                if (aircraft.RegistrationCode.StartsWith("SE"))
                {
                    availableFlights = flights.OfType<Flight>()
                        .Where(f => f.ScheduledFlightId.StartsWith("D8"))
                        .ToList();
                }
                else if (aircraft.RegistrationCode.StartsWith("LN"))
                {
                    availableFlights = flights.OfType<Flight>()
                        .Where(f => f.ScheduledFlightId.StartsWith("DY") || f.ScheduledFlightId.StartsWith("DH"))
                        .ToList();
                }

                var unassignedFlights = availableFlights.Where(f => f.AircraftId == null)
                    .OrderBy(f => f.DepartureDateTime)
                    .ThenBy(f => f.ScheduledFlightId)
                    .ToList();

                var lastArrivalTime = DateTime.MinValue;
                var lastDestination = scheduledFlights
                    .SingleOrDefault(sf => unassignedFlights.First().ScheduledFlightId == sf.FlightNumber)
                    ?.DestinationFrom;

                foreach (var flight in unassignedFlights)
                {
                    var scheduledFlight = scheduledFlights
                        .SingleOrDefault(sf => flight.ScheduledFlightId == sf.FlightNumber)
                        ?.DestinationFrom;

                    if (aircraft.Flights.Count == 0 || flight.DepartureDateTime >= lastArrivalTime.AddMinutes(25) &&
                        flight.DepartureDateTime <= lastArrivalTime.AddMinutes(600) &&
                        scheduledFlight == lastDestination)
                    {
                        flight.AircraftId = aircraft.RegistrationCode;
                        aircraft.Flights.Add(flight);
                        lastArrivalTime = flight.ArrivalDateTime ?? DateTime.MinValue;
                        lastDestination = scheduledFlights
                            .SingleOrDefault(sf => flight.ScheduledFlightId == sf.FlightNumber)
                            ?.DestinationTo;

                        var returnFlight = unassignedFlights.FirstOrDefault(f =>
                            int.Parse(f.ScheduledFlightId[2..]) == int.Parse(flight.ScheduledFlightId[2..] + 1));

                        if (int.Parse(flight.ScheduledFlightId[2..]) % 2 == 0 && returnFlight != null)
                        {
                            returnFlight.AircraftId = aircraft.RegistrationCode;
                            aircraft.Flights.Add(returnFlight);
                            lastArrivalTime = returnFlight.ArrivalDateTime ?? DateTime.MinValue;
                            lastDestination = scheduledFlights
                                .SingleOrDefault(sf => returnFlight.ScheduledFlightId == sf.FlightNumber)
                                ?.DestinationTo;
                        }
                    }
                    else if (flight.DepartureDateTime > lastArrivalTime.AddMinutes(600))
                    {
                        var selectedFlight = unassignedFlights.FirstOrDefault(result =>
                            result.DepartureDateTime >= lastArrivalTime.AddMinutes(300));

                        if (selectedFlight != null)
                        {
                            selectedFlight.AircraftId = aircraft.RegistrationCode;
                            aircraft.Flights.Add(selectedFlight);
                            lastArrivalTime = selectedFlight.ArrivalDateTime ?? DateTime.MinValue;
                            lastDestination = scheduledFlights
                                .SingleOrDefault(sf => selectedFlight.ScheduledFlightId == sf.FlightNumber)
                                ?.DestinationTo;
                        }
                    }
                }
            }

            dbContext.SaveChanges();
        }

        public void PlanFlights2()
        {
            var flights = dbContext.Flights.OfType<Flight>().ToList();
            var aircrafts = dbContext.Aircrafts.ToList();
            var scheduledFlights = dbContext.ScheduledFlights.ToList();
            var sortedFlights = flights.OrderBy(f => f.DepartureDateTime).ToList();

            foreach (var flight in sortedFlights)
            {
                if (flight.AircraftId != null) continue;
                var registrationCodePrefix = flight.ScheduledFlightId.Substring(0, 2);
                var availableAircrafts = new List<Aircraft>();

                switch (registrationCodePrefix)
                {
                    case "D8":
                        availableAircrafts = aircrafts.Where(a => a.RegistrationCode.StartsWith("SE")).ToList();
                        break;
                    case "DY" or "DH":
                        availableAircrafts = aircrafts.Where(a => a.RegistrationCode.StartsWith("LN")).ToList();
                        break;
                }

                if (availableAircrafts.All(a => a.Flights.Count != 0))
                {
                    var departureDestination =
                        scheduledFlights.SingleOrDefault(sf => flight.ScheduledFlightId == sf.FlightNumber);

                    var flightsFromSameAirport = scheduledFlights
                        .Where(sf => departureDestination?.DestinationFrom == sf.DestinationTo)
                        .ToList();

                    availableAircrafts = availableAircrafts.Where(a =>
                            a.Flights.Count > 0 &&
                            a.Flights.All(af => af.ArrivalDateTime?.AddMinutes(30) <= flight.DepartureDateTime) &&
                            a.Flights.Any(af => af.ArrivalDateTime <= flight.DepartureDateTime.AddMinutes(-30)) &&
                            a.Flights.Any(f => flightsFromSameAirport
                                .Select(ff => ff.FlightNumber)
                                .Contains(f.ScheduledFlightId)))
                        .ToList();
                }

                Aircraft? firstAvailableAircraft = null;

                if (availableAircrafts.Count > 0)
                {
                    firstAvailableAircraft = availableAircrafts.FirstOrDefault(a => a.Flights.Count == 0) ??
                                             availableAircrafts.First();
                }

                if (firstAvailableAircraft != null)
                {
                    flight.AircraftId = firstAvailableAircraft.RegistrationCode;
                    firstAvailableAircraft.Flights.Add(flight);
                }
            }

            dbContext.SaveChanges();
        }
    }
}