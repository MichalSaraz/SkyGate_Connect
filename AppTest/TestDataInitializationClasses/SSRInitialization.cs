using System.Diagnostics;
using Core.PassengerContext.Booking;
using Core.PassengerContext.Booking.Enums;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace TestProject.TestDataInitializationClasses
{
    public class SSRInitialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new();

        public SSRInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeSSR()
        {
            var passengers = dbContext.PassengerBookingDetails.ToList();

            var infants = passengers.Where(p => p.Age < 2).ToList();
            var children = passengers.Where(p => p.Age is < 12 and > 1).ToList();
            var unaccompaniedMinors = passengers.Where(p => p.Age is <= 12 and >= 5).ToList();
            var adults = passengers.Where(p => p.Age >= 18).ToList();
            var elderly = passengers.Where(p => p.Age >= 50).ToList();
            var wchPaxList = new List<PassengerBookingDetails>();

            _AssignSSR("CHLD", null, children, null);
            _AssignSSR("UMNR", null, unaccompaniedMinors, null);
            _AssignSSR("AVIH", 1500, adults, null);
            _AssignSSR("BIKE", 800, adults, null);
            _AssignSSR("BLND", 4000, passengers, null);
            _AssignSSR("CBBG", 2000, adults, null);
            _AssignSSR("EXST", 8000, adults, null);
            _AssignSSR("DEAF", 3000, passengers, null);
            _AssignSSR("DEPA", 20000, adults, null);
            _AssignSSR("DEPU", 20000, adults, null);
            _AssignSSR("DPNA", 3000, passengers, null);
            _AssignSSR("ESAN", 10000, adults, null);
            _AssignSSR("MEDA", 5000, passengers, null);
            _AssignSSR("PETC", 800, adults, null);
            _AssignSSR("SPEQ", 150, adults, null);
            _AssignSSR("SVAN", 10000, adults, null);
            _AssignSSR("WEAP", 4000, adults, null);
            _AssignSSR("WCHR", 100, elderly, wchPaxList);
            _AssignSSR("WCHS", 150, elderly, wchPaxList);
            _AssignSSR("WCHC", 1000, passengers, wchPaxList);
            _AssignSSR("XBAG", null, adults, null);
            _AssignSSR("INFT", null, infants, null);
            _AssignSSR("WCMP", 10, wchPaxList, null);
            _AssignSSR("WCLB", 40, wchPaxList, null);
            _AssignSSR("WCBD", 80, wchPaxList, null);
        }

        private void _AssignSSR(string SSR, int? SSRFrequency, List<PassengerBookingDetails> eligiblePassengers,
            ICollection<PassengerBookingDetails>? wchPaxList)
        {
            var ssrList = dbContext.SSRCodes.ToList();

            var bookingReferences = dbContext.BookingReferences
                .Include(bookingReference => bookingReference.LinkedPassengers)
                .ToList();

            var countForSSR = 0;

            if (SSR == "WCMP" || SSR == "WCBD" || SSR == "WCLB" && SSRFrequency != null)
            {
                countForSSR = eligiblePassengers.Count / SSRFrequency ?? 0;
            }
            else if (SSRFrequency != null)
            {
                countForSSR = (int)(dbContext.PassengerBookingDetails.ToList().Count / SSRFrequency);
            }
            else if (SSRFrequency == null)
            {
                if (SSR == "XBAG")
                {
                    eligiblePassengers = eligiblePassengers.Where(x => x.BaggageAllowance >= 3).ToList();
                }
                else if (SSR == "UMNR")
                {
                    eligiblePassengers = eligiblePassengers.Where(passenger =>
                        {
                            var reference =
                                bookingReferences.FirstOrDefault(reference => reference.PNR == passenger.PNRId);
                            return reference != null && reference.LinkedPassengers.Count == 1;
                        })
                        .ToList();
                }

                countForSSR = eligiblePassengers.Count;
            }

            for (int i = 0; i < countForSSR; i++)
            {
                if (eligiblePassengers != null)
                {
                    var selectedPassenger = eligiblePassengers.Where(p =>
                            p.BookedSSR == null ||
                            !p.BookedSSR.Values.Any(valueList => valueList.Any(value => value.StartsWith(SSR))))
                        .MinBy(_ => random.Next());

                    var flights = bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger?.PNRId)
                        ?.FlightItinerary;

                    if (flights != null)
                    {
                        if (SSR is "CBBG" or "EXST")
                        {
                            var passengerInfo = new PassengerBookingDetails(SSR, selectedPassenger?.LastName,
                                PaxGenderEnum.UNDEFINED, selectedPassenger?.PNRId);

                            dbContext.PassengerBookingDetails.Add(passengerInfo);
                            bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger?.PNRId)
                                ?.LinkedPassengers.Add(passengerInfo);
                        }

                        var hasWheelchairSSR = selectedPassenger?.BookedSSR.SelectMany(kv => kv.Value)
                            .Any(val => val is "WCMP" or "WCBD" or "WCLB") ?? false;

                        var hasSSR = selectedPassenger?.BookedSSR.SelectMany(kv => kv.Value)
                            .Any(val => val.StartsWith(SSR[..4])) ?? false;

                        if (SSR.StartsWith("WCH") && !hasSSR)
                        {
                            if (selectedPassenger != null) wchPaxList?.Add(selectedPassenger);
                        }

                        if (!hasSSR || !hasWheelchairSSR && SSR is "WCMP" or "WCBD" or "WCLB")
                        {
                            var selectedFlights = flights.Select(f => f.Key).ToList();

                            foreach (var flight in selectedFlights)
                            {
                                if (selectedPassenger?.BookedSSR == null)
                                {
                                    if (selectedPassenger != null)
                                        selectedPassenger.BookedSSR = new Dictionary<string, List<string>>();
                                }

                                if (selectedPassenger != null && !hasSSR)
                                {
                                    string value;

                                    var first = ssrList.FirstOrDefault(f => f.Code == SSR);

                                    if (first is { IsFreeTextMandatory: true })
                                    {
                                        value = _GenerateStringValue(SSR, selectedPassenger.Age ?? 0,
                                            selectedPassenger.FirstName);
                                    }
                                    else
                                    {
                                        value = SSR;
                                    }

                                    if (!selectedPassenger.BookedSSR.TryGetValue(flight, out var list))
                                    {
                                        list = new List<string>();
                                        selectedPassenger.BookedSSR.Add(flight, list);
                                    }

                                    list.Add(value);

                                    Trace.WriteLine($"SSR added {value}");
                                }
                            }
                        }
                    }
                }
            }

            dbContext.SaveChanges();
        }

        private string _GenerateStringValue(string ssr, int age, string firstName)
        {
            var comments = new Dictionary<string, string[]>
            {
                {
                    "avih",
                    new[]
                    {
                        "Dog 12kg container size 100x100x50",
                        "Two dogs in one cage weight up to 32kg container size 150x100x50",
                        "Dog up to 32kg container size 150x100x50", "Dog 20kg container size 120x100x50",
                        "Dog 15kg container size 100x100x50"
                    }
                },
                {
                    "cbbg",
                    new[]
                    {
                        "Guitar on extra seat weight 5kg dimensions 120x30x20",
                        "Cello on extra seat weight 8kg dimensions 150x30x20"
                    }
                },
                {
                    "depa",
                    new[]
                    {
                        "Deportee - accompanied by an escort of 2 marshals",
                        "Deportee - accompanied by an escort of 4 marshals"
                    }
                },
                { "dpna", new[] { "Customer with mental disability", "Customer with autism" } },
                { "esan", new[] { "Emotional support dog 5kg", "Emotional support cat 2kg" } },
                { "exst", new[] { "Seat for extra comfort" } },
                {
                    "speq",
                    new[]
                    {
                        "Golf bag up to 23kg dimensions not exceeding 300cm",
                        "Skis  up to 23kg dimensions not exceeding 300cm",
                        "Snowboard up to 23kg dimensions not exceeding 300cm",
                        "Kite surf up to 23kg dimensions not exceeding 300cm",
                        "Surf up to 23kg dimensions not exceeding 300cm"
                    }
                },
                {
                    "weap",
                    new[]
                    {
                        "Small Airsoft gun without ammunition", "Firearm with 8kg ammunition",
                        "Firearm without ammunition", "Sport Firearm without ammunition",
                        "Sport Firearm with 8kg ammunition"
                    }
                },
                { "umnr", new[] { $"{age} years old" } },
                { "xbag", new[] { "Extra bag up to 23kg", "Extra bag up to 32kg" } },
                { "inft", new[] { $"Name {firstName}, {age} year old" } },
            };

            string[] commentOptions = comments[ssr.ToLower()];
            string selectedComment = commentOptions[random.Next(commentOptions.Length)];
            return $"{ssr} - {selectedComment}";
        }
    }
}