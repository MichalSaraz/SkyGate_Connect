namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Represents the current system time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Parses the input string as a date, with an optional default time and format setting.
        /// </summary>
        /// <param name="input">The input string representation of the date.</param>
        /// <param name="defaultTime">The default time to use if the input does not contain a time component. Default is
        /// "0:00:00".</param>
        /// <param name="useFormatsWithYear">Specifies whether to use date formats that include the year. Default is
        /// false.</param>
        /// <returns>
        /// The parsed DateTime value, or null if the input cannot be parsed as a valid date.
        /// </returns>
        DateTime? ParseDate(string input, string defaultTime = "0:00:00", bool useFormatsWithYear = false);
    }
}
