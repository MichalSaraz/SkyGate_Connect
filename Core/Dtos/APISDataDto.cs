namespace Core.Dtos
{
    public class APISDataDto
    {
        public Guid Id { get; init; }
        public string DocumentNumber { get; init; }
        public string FirstName { get; init; }
        public string LastName { get; init; }
        public string Nationality { get; init; }
        public string CountryOfIssue { get; init; }
        public string DocumentType { get; init; }
        public string Gender { get; init; }
        public DateTime DateOfBirth { get; init; }
        public DateTime DateOfIssue { get; init; }
        public DateTime ExpirationDate { get; init; }
    }
}