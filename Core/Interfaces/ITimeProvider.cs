namespace Core.Interfaces
{
    public interface ITimeProvider
    {
        /// <summary>
        /// Represents the current system time.
        /// </summary>
        DateTime Now { get; }

        
        DateTime? ParseDate(string input, bool useFormatsWithYear = false);
    }
}
