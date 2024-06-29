using Core.Interfaces;
using System.Globalization;

namespace Core.Time
{
    public abstract class TimeProviderBase : ITimeProvider
    {
        public virtual DateTime Now => DateTime.Now;

        public virtual DateTime? ParseDate(string input, string defaultTime = "0:00:00",
            bool useFormatsWithYear = false)
        {
            string[] formatsDateOnly = { "dMMM", "DDMMM", "ddMMM" };
            string[] formatsDateWithYear = { "dMMMyyyy", "ddMMMyyyy", "DDMMMyyyy" };

            string[] formatsToUse = useFormatsWithYear ? formatsDateWithYear : formatsDateOnly;

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var isValidDate = DateTime.TryParseExact(input, formatsToUse, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out var parsedDate);
            
            if (!isValidDate)
            {
                throw new ArgumentException("Invalid date format");
            }

            TimeSpan parsedTime;
            
            if (defaultTime == "0:00:00")
            {
                parsedTime = TimeSpan.Zero;
            }
            else
            {
                var isValidTime = TimeSpan.TryParseExact(defaultTime, @"hh\:mm", CultureInfo.InvariantCulture,
                    out parsedTime);
                if (!isValidTime)
                {
                    throw new ArgumentException("Invalid time format");
                }
            }

            var result = parsedDate.Date + parsedTime;

            return ParseDateWithCurrentYear(result);
        }

        protected virtual DateTime? ParseDateWithCurrentYear(DateTime date)
        {
            return new DateTime(Now.Year, date.Month, date.Day, date.Hour, date.Minute, date.Second);
        }
    }
}