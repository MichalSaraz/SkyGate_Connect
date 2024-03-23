namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Represents the current system time.
        /// </summary>
        DateTime Now { get; }


        /// <summary>
        /// Parses the input string into a nullable DateTime object.
        /// </summary>
        /// <param name="input">The string to parse.</param>
        /// <param name="useFormatsWithYear">Indicates whether to use formats that include the year component.</param>
        /// <returns> A nullable DateTime object if the parsing was successful; otherwise, null. </returns>
        DateTime? ParseDate(string input, bool useFormatsWithYear = false);
    }
}
