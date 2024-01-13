using Core.PassengerContext.Regulatory;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Data
{
    public class APISDataInitialization
    {
        int id = 0;
        private readonly AppDbContext dbContext;
        private readonly Random random = new Random();

        public APISDataInitialization(AppDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public void GenerateAPIS()
        {
            var passengerList = dbContext.Passengers.ToList();

            var passengerListSortedByPNRId = passengerList
                .OrderBy(f => f.PNRId)
                .ToList()
                .Take((int)Math.Ceiling(passengerList.Count * 0.5));

            var passengerListWithAssignedSeats = passengerListSortedByPNRId
                .Where(f => f.AssignedSeats.Any())
                .OrderBy(f => f.PNRId)
                .ToList()
                .Take((int)Math.Ceiling(passengerList.Count * 0.9));

            var passengersSelectedForAPIS = passengerListSortedByPNRId
                .Concat(passengerListWithAssignedSeats)
                .Distinct()
                .ToList();

            var nationalityByPNRId = new Dictionary<string, Country>();
            var passportNumberCollection = new HashSet<string>();
            var passportNumberList = new List<string>();

            for (int i = 0; i < passengersSelectedForAPIS.Count; i++)
            {
                int newPassportNumber;
                do
                {
                    newPassportNumber = random.Next(10000000, 99999999);
                } while (!passportNumberCollection.Add(newPassportNumber.ToString()));

                // Přidání unikátního čísla do kolekce
                passportNumberList.Add(newPassportNumber.ToString());
            }

            var countriesDictionary = new Dictionary<string, Range>
                {
                    { "DEU", new Range(0, 1132) }, // Německo    
                    { "USA", new Range(1133, 1981) }, // Spojené státy americké    
                    { "FRA", new Range(1982, 2817) }, // Francie (1981 + 836)    
                    { "GBR", new Range(2818, 3760) }, // Spojené království (2817 + 943)    
                    { "CHN", new Range(3761, 4408) }, // Čína (3760 + 648)    
                    { "RUS", new Range(4409, 4954) }, // Rusko (4408 + 546)    
                    { "IND", new Range(4955, 5744) }, // Indie (4954 + 790)    
                    { "ESP", new Range(5745, 6423) }, // Španělsko (5744 + 679)    
                    { "ITA", new Range(6424, 7090) }, // Itálie (6423 + 667)    
                    { "POL", new Range(7091, 7805) }, // Polsko (7090 + 715)    
                    { "TUR", new Range(7806, 8182) }, // Turecko (7805 + 377)    
                    { "NLD", new Range(8183, 9128) }, // Nizozemsko (8182 + 943)    
                    { "TWN", new Range(9129, 9533) }, // Taiwan (9128 + 405)    
                    { "BRA", new Range(9534, 9916) }, // Brazílie (9533 + 383)    
                    { "UKR", new Range(9917, 10399) }, // Ukrajina (9916 + 483)    
                    { "BEL", new Range(10400, 10876) }, // Belgie (10399 + 477)    
                    { "CAN", new Range(10877, 11523) }, // Kanada (10876 + 647)    
                    { "AUS", new Range(11524, 12162) }, // Austrálie (11523 + 639)    
                    { "IRN", new Range(12163, 12347) }, // Írán (12162 + 185)    
                    { "CHE", new Range(12348, 13130) }, // Švýcarsko (12347 + 783)    
                    { "JPN", new Range(13131, 13693) }, // Japonsko (13130 + 563)    
                    { "ARG", new Range(13694, 14459) }, // Argentina (13693 + 766)    
                    { "THA", new Range(14460, 14848) }, // Thajsko (14459 + 389)    
                    { "KOR", new Range(14849, 15287) }, // Jižní Korea (14848 + 439)    
                    { "FIN", new Range(15288, 15716) }, // Finsko (15287 + 429)    
                    { "PER", new Range(15717, 16085) }, // Peru (15716 + 369)    
                    { "MKD", new Range(16086, 16298) }, // Makedonie (16085 + 213)    
                    { "EGY", new Range(16299, 16411) }, // Egypt (16298 + 113)    
                    { "ISR", new Range(16412, 16600) }, // Izrael (16411 + 189)    
                    { "ZAF", new Range(16601, 16813) }, // Jihoafrická republika (16600 + 213)    
                    { "PAK", new Range(16814, 17031) }, // Pákistán (16813 + 218)    
                    { "MEX", new Range(17032, 17371) }, // Mexiko (17031 + 340)    
                    { "IDN", new Range(17372, 17697) }, // Indonésie (17371 + 326)    
                    { "BGD", new Range(17698, 18066) }, // Bangladéš (17697 + 369)    
                    { "PHL", new Range(18067, 18435) }, // Filipíny (18066 + 369)    
                    { "VNM", new Range(18436, 18661) }, // Vietnam (18435 + 226)    
                    { "GEO", new Range(18662, 18866) }, // Gruzie (18661 + 205)    
                    { "COL", new Range(18867, 19035) }, // Kolumbie (18866 + 169)    
                    { "SRB", new Range(19036, 19319) }, // Srbsko (19035 + 284)    
                    { "CHL", new Range(19320, 19545) }, // Chile (19319 + 226)    
                    { "ROU", new Range(19546, 19828) }, // Rumunsko (19545 + 283)    
                    { "GRC", new Range(19829, 20205) }, // Řecko (19828 + 377)    
                    { "DZA", new Range(20206, 20374) }, // Alžírsko (20205 + 169)    
                    { "CZE", new Range(20375, 20713) }, // Česko (20374 + 339)    
                    { "BLR", new Range(20714, 20939) }, // Bělorusko (20713 + 226)    
                    { "MYS", new Range(20940, 21108) }, // Malajsie (20939 + 169)    
                    { "PRT", new Range(21109, 21447) }, // Portugalsko (21108 + 339)    
                    { "KAZ", new Range(21448, 21616) }, // Kazachstán (21447 + 169)    
                    { "TUN", new Range(21617, 21785) }, // Tunisko (21616 + 169)    
                    { "HKG", new Range(21786, 22149) }, // Hongkong (21785 + 226)
                    { "HUN", new Range(22150, 22521) }, // Maďarsko (22011 + 283)    
                    { "VEN", new Range(22522, 22691) }, // Venezuela (22294 + 169)    
                    { "LUX", new Range(22692, 22861) }, // Lucembursko (22463 + 169)    
                    { "JOR", new Range(22862, 23031) }, // Jordánsko (22632 + 169)    
                    { "BGR", new Range(23032, 23258) }, // Bulharsko (22801 + 226)    
                    { "OMN", new Range(23259, 23329) }, // Omán (23027 + 70)    
                    { "NGA", new Range(23330, 23499) }, // Nigérie (23097 + 169)    
                    { "ECU", new Range(23500, 23669) }, // Ekvádor (23266 + 169)    
                    { "AZE", new Range(23670, 23816) }, // Ázerbájdžán (23435 + 146)    
                    { "MAR", new Range(23817, 23943) }, // Maroko (23581 + 226)    
                    { "SGP", new Range(23944, 24170) }, // Singapur (23807 + 226)    
                    { "SAU", new Range(24171, 24340) }, // Saúdská Arábie (24033 + 169)    
                    { "GHA", new Range(24341, 24486) }, // Ghana (24202 + 145)    
                    { "SEN", new Range(24487, 24632) }, // Senegal (24347 + 145)    
                    { "CMR", new Range(24633, 24778) }, // Kamerun (24492 + 145)    
                    { "ALB", new Range(24779, 24948) }, // Albánie (24637 + 169)    
                    { "LKA", new Range(24949, 25118) }, // Srí Lanka (24806 + 169)    
                    { "CUB", new Range(25119, 25245) }, // Kuba (24975 + 226)    
                    { "NZL", new Range(25246, 25415) }, // Nový Zéland (25201 + 169)    
                    { "ARE", new Range(25416, 25585) }, // Arabské emiráty (25370 + 169)    
                    { "QAT", new Range(25586, 25755) }, // Katar (25539 + 169)    
                    { "AUT", new Range(25756, 26139) }, // Rakousko (25708 + 383)    
                    { "IRL", new Range(26140, 26423) }, // Irsko (26091 + 283)    
                    { "UGA", new Range(26424, 26707) }, // Uganda (26374 + 283)    
                    { "HRV", new Range(26708, 26991) }, // Chorvatsko (26657 + 283)    
                    { "SVN", new Range(26992, 27161) }, // Slovinsko (26940 + 169)    
                    { "EST", new Range(27162, 27331) }, // Estonsko (27109 + 169)    
                    { "LVA", new Range(27332, 27501) }, // Lotyšsko (27278 + 169)    
                    { "LTU", new Range(27502, 27671) }, // Litva (27447 + 169)    
                    { "SVK", new Range(27672, 27898) }, // Slovensko (27616 + 226)    
                    { "BIH", new Range(27899, 28068) }, // Bosna a Hercegovina (27842 + 169)    
                    { "ARM", new Range(28069, 28238) }, // Arménie (28011 + 169)    
                    { "TZA", new Range(28239, 28408) }, // Tanzanie (28180 + 169)    
                    { "CYP", new Range(28409, 28578) }, // Kypr (28349 + 169)    
                    { "UZB", new Range(28579, 28748) }, // Uzbekistán (28518 + 169)    
                    { "MDA", new Range(28749, 28918) }, // Moldavsko (28687 + 169)    
                    { "LBN", new Range(28919, 29088) }, // Libanon (28856 + 169)    
                    { "LBY", new Range(29089, 29258) }, // Libye (29194 + 169)    
                    { "SYR", new Range(29259, 29428) }, // Sýrie (29363 + 169)    
                    { "BHR", new Range(29429, 29598) }, // Bahrajn (29363 + 169)    
                    { "KWT", new Range(29599, 29768) }, // Kuvajt (29532 + 169)    
                    { "MNE", new Range(29769, 29938) }, // Černá Hora (29701 + 169)    
                    { "SLV", new Range(29939, 30108) }, // Salvador (29870 + 169)    
                    { "CRI", new Range(30109, 30278) }, // Kostarika (30039 + 169)    
                    { "URY", new Range(30279, 30448) }, // Uruguay (30377 + 169)    
                    { "PAN", new Range(30449, 30618) }, // Panama (30546 + 169)    
                    { "MLT", new Range(30619, 30788) }, // Malta (30546 + 169)    
                    { "ISL", new Range(30789, 30958) }, // Island (30715 + 169)
                    { "DNK", new Range(30959, 34125) }, // Dánsko (30546 + 3562)    
                    { "NOR", new Range(34126, 41215) }, // Norsko (30546 + 169)    
                    { "SWE", new Range(41216, 47999) }, // Švédsko
                };
            

            foreach (var passenger in passengersSelectedForAPIS)
            {
                if (passenger.TravelDocuments == null)
                {
                    passenger.TravelDocuments = new List<APISData>();
                }
                                
                int randomNumber = random.Next(0, 48000);

                var randomCountrySelection = countriesDictionary
                    .FirstOrDefault(kvp => randomNumber >= kvp.Value.LowerBound && randomNumber <= kvp.Value.UpperBound);
                var selectedCountry = dbContext.Countries.FirstOrDefault(f => f.Country3LetterCode == randomCountrySelection.Key);

                if (!nationalityByPNRId.ContainsKey(passenger.PNRId))
                {
                    nationalityByPNRId[passenger.PNRId] = selectedCountry;
                }
                else
                {
                    selectedCountry = nationalityByPNRId[passenger.PNRId];
                }

                DateTime currentDate = new DateTime(2023, 10, 14);

                DateTime dateOfBirth = currentDate.AddYears(-passenger.Age);
                dateOfBirth = dateOfBirth.AddDays(-random.Next(0, 366));

                DateTime dateOfIssue = currentDate.AddYears(-random.Next(0, 10));
                dateOfIssue = dateOfIssue.AddDays(-random.Next(0, 366));

                DateTime expirationDate = dateOfIssue.AddYears(10);

                var newAPISData = new APISData
                {
                    Id = id,
                    DocumentNumber = passportNumberList[id],
                    Passenger = passenger,
                    PassengerId = passenger.Id,
                    Nationality = selectedCountry,
                    IssueCountry = selectedCountry,
                    IssueCountryId = selectedCountry.Country2LetterCode,
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
        public int LowerBound { get; set; }
        public int UpperBound { get; set; }

        public Range(int lowerBound, int upperBound)
        {
            LowerBound = lowerBound;
            UpperBound = upperBound;
        }
    }
}
