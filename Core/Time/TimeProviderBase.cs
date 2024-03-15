using Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Time
{
    public abstract class TimeProviderBase : ITimeProvider
    {
        public virtual DateTime Now => DateTime.Now;

        public virtual DateTime? ParseDate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            string[] formats = { "dMMM", "DDMMM", "ddMMM" };

            var isValid = DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);

            if (isValid)
            {
                return ParseDateWithCurrentYear(result);
            }

            throw new ArgumentException("Invalid date format");   
        }

        protected virtual DateTime? ParseDateWithCurrentYear(DateTime date)
        {
            return new DateTime(Now.Year, date.Month, date.Day).Date;
        }
    }
}
