using Core.Interfaces;
using System.Globalization;

namespace Core.Time
{
    public abstract class TimeProviderBase : ITimeProvider
    {
        public virtual DateTime Now => DateTime.Now;

        /// <summary>
        /// Parses the input string into a DateTime object.
        /// </summary>
        /// <param name="input">The string to parse into a DateTime object.</param>
        /// <param name="useFormatsWithYear">Optional. Specifies whether to use formats with year or not. Default
        /// is false.</param>
        /// <returns> If the input string is a valid date format, returns the parsed DateTime object. Otherwise,
        /// throws an ArgumentException with the message "Invalid date format". </returns>
        public virtual DateTime? ParseDate(string input, bool useFormatsWithYear = false)
        {
            string[] formatsDateOnly = { "dMMM", "DDMMM", "ddMMM" };
            string[] formatsDateWithYear = { "dMMMyyyy", "ddMMMyyyy", "DDMMMyyyy" };

            string[] formatsToUse = useFormatsWithYear ? formatsDateWithYear : formatsDateOnly;

            if (string.IsNullOrEmpty(input))
            {
                return null;
            }

            var isValid = DateTime.TryParseExact(input, formatsToUse, CultureInfo.InvariantCulture, DateTimeStyles.None,
                out var result);

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