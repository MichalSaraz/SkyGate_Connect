using Core.FlightContext;
using Core.PassengerContext;
using Core.PassengerContext.Booking;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.Intrinsics.X86;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class SSRInicialization
    {
        private readonly AppDbContext dbContext;
        private readonly Random random = new Random();

        public SSRInicialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void InitializeSSR()
        {
            var passengers = dbContext.PassengerInfos.ToList();

            var infants = passengers.Where(p => p.Age < 2).ToList();
            var children = passengers.Where(p => p.Age < 12 && p.Age > 1).ToList();
            var umnrs = passengers.Where(p => p.Age <= 12 && p.Age >= 5).ToList();
            var adults = passengers.Where(p => p.Age >= 18).ToList();
            var elderly = passengers.Where(p => p.Age >= 50).ToList();
            var wchPaxList = new List<PassengerInfo>();

            AssignSSR("CHLD", null, children, null);
            AssignSSR("UMNR", null, umnrs, null);
            AssignSSR("AVIH", 1500, adults, null);
            AssignSSR("BIKE", 800, adults, null);
            AssignSSR("BLND", 4000, passengers, null);
            AssignSSR("CBBG", 2000, adults, null);
            AssignSSR("EXST", 8000, adults, null);
            AssignSSR("DEAF", 3000, passengers, null);
            AssignSSR("DEPA", 20000, adults, null);
            AssignSSR("DEPU", 20000, adults, null);
            AssignSSR("DPNA", 3000, passengers, null);
            AssignSSR("ESAN", 10000, adults, null);
            AssignSSR("MEDA", 5000, passengers, null);
            AssignSSR("PETC", 800, adults, null);
            AssignSSR("SPEQ", 150, adults, null);
            AssignSSR("SVAN", 10000, adults, null);
            AssignSSR("WEAP", 4000, adults, null);
            AssignSSR("WCHR", 100, elderly, wchPaxList);
            AssignSSR("WCHS", 150, elderly, wchPaxList);
            AssignSSR("WCHC", 1000, passengers, wchPaxList);
            AssignSSR("XBAG", null, adults, null);
            AssignSSR("INFT", null, infants, null);
            AssignSSR("WCMP", 10, wchPaxList, null);
            AssignSSR("WCLB", 40, wchPaxList, null);
            AssignSSR("WCBD", 80, wchPaxList, null);

            TransferValuesToBookedSSRColumn();
        }

        public void AssignSSR(string SSR, int? SSRFrequency, List<PassengerInfo> eligiblePassengers, List<PassengerInfo> wchPaxList)
        {
            var ssrList = dbContext.SSRCodes.ToList();
            var bookingReferences = dbContext.BookingReferences.ToList();
            var countForSSR = 0;

            if (SSR == "WCMP" || SSR == "WCBD" || SSR == "WCLB" && SSRFrequency != null)
            {
                countForSSR = (int)(eligiblePassengers.Count / SSRFrequency);
            }
            else if (SSRFrequency != null)
            {
                countForSSR = (int)(dbContext.PassengerInfos.ToList().Count / SSRFrequency);
            }
            else if (SSRFrequency == null)
            {

                if (SSR == "XBAG")
                {
                    eligiblePassengers = eligiblePassengers
                        .Where(x => x.BaggageAllowance >= 3)
                        .ToList();
                }
                else if (SSR == "UMNR")
                {
                    eligiblePassengers = eligiblePassengers
                        .Where(passenger =>
                        {
                            var reference = bookingReferences
                                .FirstOrDefault(reference => reference.PNR == passenger.PNRId);
                            return reference != null && reference.LinkedPassengers.Count == 1;
                        })
                        .ToList();
                }
                countForSSR = eligiblePassengers.Count;
            }

            for (int i = 0; i < countForSSR; i++)
            {
                var selectedPassenger = eligiblePassengers
                    .Where(p => p.BookedSSRList == null || p.BookedSSRList
                    .Any(ssr => ssr.Substring(0, 4) != SSR))
                    .OrderBy(_ => random.Next())
                    .FirstOrDefault();

                var flights = bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger.PNRId)?.FlightItinerary;

                if (flights != null)
                {
                    if (SSR == "CBBG" || SSR == "EXST")
                    {
                        int maxId = dbContext.PassengerInfos.Max(p => p.Id);
                        int newId = maxId + i + 1;
                        PassengerInfo passengerInfo = new PassengerInfo(SSR, selectedPassenger.LastName, PaxGenderEnum.UNDEFINED, selectedPassenger.PNRId, newId);
                        dbContext.PassengerInfos.Add(passengerInfo);
                        bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger.PNRId).LinkedPassengers.Add(passengerInfo);
                    }

                    var hasWCSSR = selectedPassenger.BookedSSRList?.Any(val => val == "WCMP" || val == "WCBD" || val == "WCLB") ?? false;
                    // Check if the passenger already has the specified SSR
                    var hasSSR = selectedPassenger.BookedSSRList?.Any(val => val.StartsWith(SSR.Substring(0, 4))) ?? false;

                    // Special rules for WCHR, WCHS, and WCHC
                    if (SSR.StartsWith("WCH") && !hasSSR)
                    {
                        wchPaxList.Add(selectedPassenger);
                    }

                    if (!hasSSR || !hasWCSSR && (SSR == "WCMP" || SSR == "WCBD" || SSR == "WCLB"))
                    {
                        var selectedFlights = flights?.Select(f => f.Key).ToList();

                        foreach (var flight in selectedFlights)
                        {
                            if (selectedPassenger.BookedSSRList == null)
                            {
                                selectedPassenger.BookedSSRList = new List<string>();
                            }

                            if (!selectedPassenger.BookedSSRList.Any(val => val.StartsWith(SSR.Substring(0, 4))))
                            {
                                // Add SSR based on special rules or generate SSR value
                                string value;
                                if (ssrList.FirstOrDefault(f => f.Code == SSR).IsFreeTextMandatory)
                                {
                                    value = GenerateStringValue(SSR, selectedPassenger.Age, selectedPassenger.FirstName);
                                }
                                else
                                {
                                    value = SSR;
                                }

                                selectedPassenger.BookedSSRList.Add(value);
                                Trace.WriteLine($"SSR added {value}");
                            }
                        }
                    }
                }
            }
            dbContext.SaveChanges();
        }

        private string GenerateStringValue(string ssr, int age, string firstName)
        {
            var comments = new Dictionary<string, string[]>
            {
                { "avih", new string[]
                    {
                        "Dog 12kg container size 100x100x50",
                        "Two dogs in one cage weight up to 32kg container size 150x100x50",
                        "Dog up to 32kg container size 150x100x50",
                        "Dog 20kg container size 120x100x50",
                        "Dog 15kg container size 100x100x50"
                    }
                },
                { "cbbg", new string[]
                    {
                        "Guitar on extra seat weight 5kg dimensions 120x30x20",
                        "Cello on extra seat weight 8kg dimensions 150x30x20"
                    }
                },
                { "depa", new string[]
                    {
                        "Deportee - accompanied by an escort of 2 marshals",
                        "Deportee - accompanied by an escort of 4 marshals"
                    }
                },
                { "dpna", new string[]
                    {
                        "Customer with mental disability",
                        "Customer with autism"
                    }
                },
                { "esan", new string[]
                    {
                        "Emotional support dog 5kg",
                        "Emotional support cat 2kg"
                    }
                },
                { "exst", new string[]
                    {
                        "Seat for extra comfort"
                    }
                },
                { "speq", new string[]
                    {
                        "Golfbag up to 23kg dimensions not exceeding 300cm",
                        "Skis  up to 23kg dimensions not exceeding 300cm",
                        "Snowboard up to 23kg dimensions not exceeding 300cm",
                        "Kitesurf up to 23kg dimensions not exceeding 300cm",
                        "Surf up to 23kg dimensions not exceeding 300cm"
                    }
                },
                { "weap", new string[]
                    {
                        "Small Airsoft gun without ammunition",
                        "Firearm with 8kg ammunition",
                        "Firearm without ammunition",
                        "Sport Firearm without ammunition",
                        "Sport Firearm with 8kg ammunition"
                    }
                },
                { "umnr", new string[]
                    {
                        $"{age} years old"
                    }
                },
                { "xbag", new string[]
                    {
                        "Extra bag up to 23kg",
                        "Extra bag up to 32kg"
                    }
                },
                { "inft", new string[]
                    {
                        $"Name {firstName}, {age} year old"
                    }
                },
            };

            string[] commentOptions = comments[ssr.ToLower()];
            string selectedComment = commentOptions[random.Next(commentOptions.Length)];
            return $"{ssr} - {selectedComment}";
        }

        private void TransferValuesToBookedSSRColumn()
        {
            var passengerInfos = dbContext.PassengerInfos.ToList();
            var bookingReferences = dbContext.BookingReferences.ToList();

            foreach (var list in passengerInfos)
            {
                var flights = bookingReferences.FirstOrDefault(f => f.PNR == list.PNRId).FlightItinerary.Select(f => f.Key).ToList();

                for (int i = 0; i < flights.Count; i++)
                {

                    if (list.BookedSSR == null)
                    {
                        list.BookedSSR = new Dictionary<string, List<string>>
                        {
                            { flights[i], (list.BookedSSRList) }
                        };
                    }
                    else if (!list.BookedSSR.ContainsKey(flights[i]))
                    {
                        list.BookedSSR[flights[i]] = list.BookedSSRList;
                    }
                    if (list.BookedSSRList != null)
                    {
                        Trace.WriteLine($"Added key {flights[i]} and list with {list.BookedSSRList.Count} SSRs");
                    }                    
                }
            }
            dbContext.SaveChanges();
        }


        //private void DeleteEXSTAndCBBGInstances()
        //{
        //    var pnrs = dbContext.BookingReferences.Where(f => f.LinkedPassengers.Any(r => r.Gender == PaxGenderEnum.UNDEFINED));
        //    foreach (var passenger in pnrs)
        //    {

        //        var item = passenger.LinkedPassengers.FirstOrDefault(s => s.Gender == PaxGenderEnum.UNDEFINED);
        //        passenger.LinkedPassengers.Remove(item);

        //    }
        //    dbContext.SaveChanges();
        //}



        //  --------------------------------------------------------------------------------------------------------------------------



        //public void AssignSSR(string SSR, int? SSRFrequency, List<PassengerInfo> eligiblePassengers, List<PassengerInfo> wchPaxList)
        //{
        //    var ssrList = dbContext.SSRCodes.ToList();
        //    var bookingReferences = dbContext.BookingReferences.ToList();
        //    var countForSSR = 0;

        //    if (SSR == "WCMP" || SSR == "WCBD" || SSR == "WCLB" && SSRFrequency != null)
        //    {
        //        countForSSR = (int)(eligiblePassengers.Count / SSRFrequency);
        //    }
        //    else if (SSRFrequency != null)
        //    {
        //        countForSSR = (int)(dbContext.PassengerInfos.ToList().Count / SSRFrequency);
        //    }
        //    else if (SSRFrequency == null)
        //    {

        //        if (SSR == "XBAG")
        //        {
        //            eligiblePassengers = eligiblePassengers
        //                .Where(x => x.BaggageAllowance >= 3)
        //                .ToList();
        //        }
        //        else if (SSR == "UMNR")
        //        {
        //            eligiblePassengers = eligiblePassengers
        //                .Where(passenger =>
        //                {
        //                    var reference = bookingReferences
        //                        .FirstOrDefault(reference => reference.PNR == passenger.PNRId);
        //                    return reference != null && reference.LinkedPassengers.Count == 1;
        //                })
        //                .ToList();
        //        }
        //        countForSSR = eligiblePassengers.Count;
        //    }

        //    for (int i = 0; i < countForSSR; i++)
        //    {
        //        var selectedPassenger = eligiblePassengers
        //            .Where(p => p.BookedSSR?.Values
        //                .SelectMany(v => v)
        //                .All(ssr => ssr.Substring(0, 4) != SSR) ?? true)
        //            .OrderBy(_ => random.Next())
        //            .FirstOrDefault();

        //        var flights = bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger.PNRId)?.FlightItinerary;

        //        if (flights != null)
        //        {
        //            if (SSR == "CBBG" || SSR == "EXST")
        //            {
        //                int maxId = dbContext.PassengerInfos.Max(p => (int?)p.Id) ?? 0;
        //                int newId = maxId + i + 1;
        //                PassengerInfo passengerInfo = new PassengerInfo(SSR, selectedPassenger.LastName, PaxGenderEnum.UNDEFINED, selectedPassenger.PNRId, newId);
        //                dbContext.PassengerInfos.Add(passengerInfo);
        //                bookingReferences.FirstOrDefault(f => f.PNR == selectedPassenger.PNRId).LinkedPassengers.Add(passengerInfo);
        //            }

        //            var hasWCSSR = selectedPassenger.BookedSSR?.Values.SelectMany(v => v).Any(val => val == "WCMP" || val == "WCBD" || val == "WCLB") ?? false;
        //            // Check if the passenger already has the specified SSR
        //            var hasSSR = selectedPassenger.BookedSSR?.Values.SelectMany(v => v).Any(val => val.StartsWith(SSR.Substring(0, 4))) ?? false;

        //            // Special rules for WCHR, WCHS, and WCHC
        //            if (SSR.StartsWith("WCH") && !hasSSR)
        //            {
        //                wchPaxList.Add(selectedPassenger);
        //            }

        //            if (!hasSSR || !hasWCSSR && (SSR == "WCMP" || SSR == "WCBD" || SSR == "WCLB"))
        //            {
        //                var selectedFlights = flights?.Select(f => f.Key).ToList();

        //                foreach (var flight in selectedFlights)
        //                {
        //                    string value;
        //                    if (ssrList.FirstOrDefault(f => f.Code == SSR).IsFreeTextMandatory)
        //                    {
        //                        value = GenerateStringValue(SSR, selectedPassenger.Age, selectedPassenger.FirstName);
        //                    }
        //                    else
        //                    {
        //                        value = SSR;
        //                    }

        //                    //dodelat podminku - pokud pred updatem nebude cely slovnik anebo jen klic , tak pridat / vytvorit
        //                    var valueBeforeUpdate = dbContext.PassengerInfos.FirstOrDefault(f => f.PNRId == selectedPassenger.PNRId);
        //                    if (valueBeforeUpdate?.BookedSSR == null)
        //                    {
        //                        valueBeforeUpdate.BookedSSR = new Dictionary<string, List<string>>
        //                        {
        //                            { flight, new List<string> { value } }
        //                        };
        //                    }
        //                    else if (!valueBeforeUpdate.BookedSSR.ContainsKey(flight))
        //                    {
        //                        valueBeforeUpdate.BookedSSR[flight] = new List<string> { value };
        //                    }
        //                    else if (!valueBeforeUpdate.BookedSSR.TryGetValue(flight, out var values) || !values.Any(val => val.StartsWith(SSR.Substring(0, 4))))
        //                    {
        //                        selectedPassenger.BookedSSR[flight].Add(value);
        //                    }

        //                    Trace.WriteLine($"SSR added {value}");
        //                }
        //            }
        //        }
        //    }
        //    dbContext.SaveChanges();
        //} 
    }
}
