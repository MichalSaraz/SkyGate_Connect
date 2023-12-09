using Core.PassengerContext.Booking;
using Core.PassengerContext;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestProject
{
    public class SSRInitializaitonTest
    {
        [Fact]
        public void GeneratePassengerItinerary()
        {
            // Načtení connection string z appsettings.json
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.Development.json")
                .Build();

            // Vytvoření instance DbContextOptions s použitím connection string
            var options = new DbContextOptionsBuilder<AppDbContext>()
                .UseNpgsql(config.GetConnectionString("DefaultConnection"))
                .Options;

            using (var dbContext = new AppDbContext(options, config))
            {
                // Vytvoření instance vaší služby s testovacím `testTimeProvider` a `dbContext`
                var service = new SSRInicialization(dbContext);

                // Spuštění metody pro vytvoření letů pro příštích 7 dní
                service.InitializeSSR();
            }
        }
    }
}















//int i = 1;
//foreach (var passenger in passengers.Where(p => p.BookedSSRList != null))
//{
//    foreach (var ssr in passenger.BookedSSRList)
//    {
//        if (ssr.StartsWith("CBBG") || ssr.StartsWith("EXST"))
//        {
//            int maxId = passengers.Max(p => p.Id);
//            int newId = maxId + i;
//            PassengerInfo passengerInfo = new PassengerInfo(ssr, passenger.LastName, PaxGenderEnum.UNDEFINED, passenger.PNRId, newId);
//            dbContext.PassengerInfos.Add(passengerInfo);
//            dbContext.BookingReferences.FirstOrDefault(f => f.PNR == passenger.PNRId).LinkedPassengers.Add(passengerInfo);
//            i++;
//        }
//    }
//}
//dbContext.SaveChanges();







