namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Represents the current system time.
        /// </summary>
        DateTime Now { get; }


        DateTime? ParseDate(string input, string defaultTime = "0:00:00", bool useFormatsWithYear = false);
    }
}
