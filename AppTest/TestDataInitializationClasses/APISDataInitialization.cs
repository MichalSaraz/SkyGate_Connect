using System.Diagnostics;
using Core.PassengerContext.APIS;
using Core.PassengerContext.APIS.Enums;
using Infrastructure.Data;

namespace TestProject.TestDataInitializationClasses
{
    public class APISDataInitialization
    {
        private int id;
        private readonly AppDbContext dbContext;
        private readonly Random random = new();

        public APISDataInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void GenerateAPIS()
        {
            var passengerList = dbContext.Passengers.ToList();

            var passengerListSortedByPNRId = passengerList
                .OrderBy(f => f.PNRId)
                .Take((int)Math.Ceiling(passengerList.Count * 0.5))
                .ToList();

            var passengerListWithAssignedSeats = passengerListSortedByPNRId
                .Where(f => f.AssignedSeats.Any())
                .OrderBy(f => f.PNRId)
                .Take((int)Math.Ceiling(passengerList.Count * 0.9))
                .ToList();

            var passengersSelectedForAPIS = passengerListSortedByPNRId
                .Concat(passengerListWithAssignedSeats)
                .Distinct()
                .ToList();

            var nationalityByPNRId = new Dictionary<string, Country?>();
            var passportNumberCollection = new HashSet<string>();
            var passportNumberList = new List<string>();

            for (int i = 0; i < passengersSelectedForAPIS.Count; i++)
            {
                int newPassportNumber;
                do
                {
                    newPassportNumber = random.Next(10000000, 99999999);
                } while (!passportNumberCollection.Add(newPassportNumber.ToString()));

                passportNumberList.Add(newPassportNumber.ToString());
            }

            var countriesDictionary = new Dictionary<string, Range>
            {
                { "DEU", new Range(0, 1132) }, // Germany
                { "USA", new Range(1133, 1981) }, // United States of America
                { "FRA", new Range(1982, 2817) }, // France
                { "GBR", new Range(2818, 3760) }, // United Kingdom
                { "CHN", new Range(3761, 4408) }, // China
                { "RUS", new Range(4409, 4954) }, // Russia
                { "IND", new Range(4955, 5744) }, // India
                { "ESP", new Range(5745, 6423) }, // Spain
                { "ITA", new Range(6424, 7090) }, // Italy
                { "POL", new Range(7091, 7805) }, // Poland
                { "TUR", new Range(7806, 8182) }, // Turkey
                { "NLD", new Range(8183, 9128) }, // Netherlands
                { "TWN", new Range(9129, 9533) }, // Taiwan
                { "BRA", new Range(9534, 9916) }, // Brazil
                { "UKR", new Range(9917, 10399) }, // Ukraine
                { "BEL", new Range(10400, 10876) }, // Belgium
                { "CAN", new Range(10877, 11523) }, // Canada
                { "AUS", new Range(11524, 12162) }, // Australia
                { "IRN", new Range(12163, 12347) }, // Iran
                { "CHE", new Range(12348, 13130) }, // Switzerland
                { "JPN", new Range(13131, 13693) }, // Japan
                { "ARG", new Range(13694, 14459) }, // Argentina
                { "THA", new Range(14460, 14848) }, // Thailand
                { "KOR", new Range(14849, 15287) }, // South Korea
                { "FIN", new Range(15288, 15716) }, // Finland
                { "PER", new Range(15717, 16085) }, // Peru
                { "MKD", new Range(16086, 16298) }, // North Macedonia
                { "EGY", new Range(16299, 16411) }, // Egypt
                { "ISR", new Range(16412, 16600) }, // Israel
                { "ZAF", new Range(16601, 16813) }, // South Africa
                { "PAK", new Range(16814, 17031) }, // Pakistan
                { "MEX", new Range(17032, 17371) }, // Mexico
                { "IDN", new Range(17372, 17697) }, // Indonesia
                { "BGD", new Range(17698, 18066) }, // Bangladesh
                { "PHL", new Range(18067, 18435) }, // Philippines
                { "VNM", new Range(18436, 18661) }, // Vietnam
                { "GEO", new Range(18662, 18866) }, // Georgia
                { "COL", new Range(18867, 19035) }, // Colombia
                { "SRB", new Range(19036, 19319) }, // Serbia
                { "CHL", new Range(19320, 19545) }, // Chile
                { "ROU", new Range(19546, 19828) }, // Romania
                { "GRC", new Range(19829, 20205) }, // Greece
                { "DZA", new Range(20206, 20374) }, // Algeria
                { "CZE", new Range(20375, 20713) }, // Czech Republic
                { "BLR", new Range(20714, 20939) }, // Belarus
                { "MYS", new Range(20940, 21108) }, // Malaysia
                { "PRT", new Range(21109, 21447) }, // Portugal
                { "KAZ", new Range(21448, 21616) }, // Kazakhstan
                { "TUN", new Range(21617, 21785) }, // Tunisia
                { "HKG", new Range(21786, 22149) }, // Hong Kong
                { "HUN", new Range(22150, 22521) }, // Hungary
                { "VEN", new Range(22522, 22691) }, // Venezuela
                { "LUX", new Range(22692, 22861) }, // Luxembourg
                { "JOR", new Range(22862, 23031) }, // Jordan
                { "BGR", new Range(23032, 23258) }, // Bulgaria
                { "OMN", new Range(23259, 23329) }, // Oman
                { "NGA", new Range(23330, 23499) }, // Nigeria
                { "ECU", new Range(23500, 23669) }, // Ecuador
                { "AZE", new Range(23670, 23816) }, // Azerbaijan
                { "MAR", new Range(23817, 23943) }, // Morocco
                { "SGP", new Range(23944, 24170) }, // Singapore
                { "SAU", new Range(24171, 24340) }, // Saudi Arabia
                { "GHA", new Range(24341, 24486) }, // Ghana
                { "SEN", new Range(24487, 24632) }, // Senegal
                { "CMR", new Range(24633, 24778) }, // Cameroon
                { "ALB", new Range(24779, 24948) }, // Albania
                { "LKA", new Range(24949, 25118) }, // Sri Lanka
                { "CUB", new Range(25119, 25245) }, // Cuba
                { "NZL", new Range(25246, 25415) }, // New Zealand
                { "ARE", new Range(25416, 25585) }, // United Arab Emirates
                { "QAT", new Range(25586, 25755) }, // Qatar
                { "AUT", new Range(25756, 26139) }, // Austria
                { "IRL", new Range(26140, 26423) }, // Ireland
                { "UGA", new Range(26424, 26707) }, // Uganda
                { "HRV", new Range(26708, 26991) }, // Croatia
                { "SVN", new Range(26992, 27161) }, // Slovenia
                { "EST", new Range(27162, 27331) }, // Estonia
                { "LVA", new Range(27332, 27501) }, // Latvia
                { "LTU", new Range(27502, 27671) }, // Lithuania
                { "SVK", new Range(27672, 27898) }, // Slovakia
                { "BIH", new Range(27899, 28068) }, // Bosnia and Herzegovina
                { "ARM", new Range(28069, 28238) }, // Armenia
                { "TZA", new Range(28239, 28408) }, // Tanzania
                { "CYP", new Range(28409, 28578) }, // Cyprus
                { "UZB", new Range(28579, 28748) }, // Uzbekistan
                { "MDA", new Range(28749, 28918) }, // Moldova
                { "LBN", new Range(28919, 29088) }, // Lebanon
                { "LBY", new Range(29089, 29258) }, // Libya
                { "SYR", new Range(29259, 29428) }, // Syria
                { "BHR", new Range(29429, 29598) }, // Bahrain
                { "KWT", new Range(29599, 29768) }, // Kuwait
                { "MNE", new Range(29769, 29938) }, // Montenegro
                { "SLV", new Range(29939, 30108) }, // El Salvador
                { "CRI", new Range(30109, 30278) }, // Costa Rica
                { "URY", new Range(30279, 30448) }, // Uruguay
                { "PAN", new Range(30449, 30618) }, // Panama
                { "MLT", new Range(30619, 30788) }, // Malta
                { "ISL", new Range(30789, 30958) }, // Iceland
                { "DNK", new Range(30959, 34125) }, // Denmark
                { "NOR", new Range(34126, 41215) }, // Norway
                { "SWE", new Range(41216, 47999) }, // Sweden
            };

            foreach (var passenger in passengersSelectedForAPIS)
            {
                passenger.TravelDocuments ??= new List<APISData>();

                var randomNumber = random.Next(0, 48000);

                var randomCountrySelection = countriesDictionary.FirstOrDefault(kvp =>
                    randomNumber >= kvp.Value.LowerBound && randomNumber <= kvp.Value.UpperBound);

                var selectedCountry =
                    dbContext.Countries.FirstOrDefault(f => f.Country3LetterCode == randomCountrySelection.Key);

                if (!nationalityByPNRId.TryGetValue(passenger.PNRId, out var value))
                {
                    nationalityByPNRId[passenger.PNRId] = selectedCountry;
                }
                else
                {
                    selectedCountry = value;
                }

                var currentDate = new DateTime(2023, 10, 14);

                var dateOfBirth = currentDate.AddYears(-passenger.Age);
                dateOfBirth = dateOfBirth.AddDays(-random.Next(0, 366));

                var dateOfIssue = currentDate.AddYears(-random.Next(0, 10));
                dateOfIssue = dateOfIssue.AddDays(-random.Next(0, 366));

                var expirationDate = dateOfIssue.AddYears(10);

                var newAPISData = new APISData
                {
                    Id = id,
                    DocumentNumber = passportNumberList[id],
                    Passenger = passenger,
                    PassengerId = passenger.Id,
                    Nationality = selectedCountry,
                    IssueCountry = selectedCountry,
                    IssueCountryId = selectedCountry?.Country2LetterCode,
                    DocumentType = DocumentTypeEnum.NormalPassport,
                    Gender = passenger.Gender,
                    FirstName = passenger.FirstName,
                    LastName = passenger.LastName,
                    DateOfBirth = dateOfBirth,
                    DateOfIssue = dateOfIssue,
                    ExpirationDate = expirationDate,
                };

                Trace.WriteLine($"APIS created {id}");
                var existingEntity = dbContext.APISData.Find(newAPISData.Id);

                if (existingEntity != null)
                {
                    newAPISData.Id += 1;
                }

                dbContext.APISData.Add(newAPISData);

                id++;
            }

            dbContext.SaveChanges();
        }
    }

    public class Range
    {
        public int LowerBound { get; }
        public int UpperBound { get; }

        public Range(int lowerBound, int upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }
    }
}