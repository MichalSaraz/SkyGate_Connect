namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Represents the current system time.
        /// </summary>
        DateTime Now { get; }

        /// <summary>
        /// Parses a date string into a nullable DateTime object.
        /// </summary>
        /// <param name="input">The date string to be parsed.</param>
        /// <returns>A nullable DateTime object representing the parsed date, or null if the input is not a valid date.
        /// </returns>
        DateTime? ParseDate(string input);
    }
}
