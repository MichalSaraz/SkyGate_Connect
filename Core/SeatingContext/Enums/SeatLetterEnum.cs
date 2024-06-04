namespace Core.SeatingContext.Enums
{
    /// <summary>
    /// Enumeration representing the position of a seat in a row.
    /// </summary>
    [Flags]
    public enum SeatLetterEnum
    {
        None = 0,
        A = 1,    // 2^0
        B = 2,    // 2^1
        C = 4,    // 2^2
        D = 8,    // 2^3
        E = 16,   // 2^4
        F = 32,   // 2^5
        G = 64,   // 2^6
        H = 128,  // 2^7
        J = 256,  // 2^8
        K = 512,  // 2^9
        L = 1024  // 2^10
    }
}
