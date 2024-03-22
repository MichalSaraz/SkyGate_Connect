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

        public virtual DateTime? ParseDate(string input, bool useFormatsWithYear = false)
        {
            string[] formatsDateOnly = { "dMMM", "DDMMM", "ddMMM" };
            string[] formatsDateWithYear = { "dMMMyyyy", "ddMMMyyyy", "DDMMMyyyy" };

            string[] formatsToUse = useFormatsWithYear ? formatsDateWithYear : formatsDateOnly;

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var isValid = DateTime.TryParseExact(input, formatsToUse, CultureInfo.InvariantCulture, DateTimeStyles.None, out var result);

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
