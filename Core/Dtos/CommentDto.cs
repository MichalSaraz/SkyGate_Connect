namespace Core.Dtos
{
    public class CommentDto
    {
        public int Id { get; init; }
        public Guid PassengerId { get; init; }
        public string CommentType { get; init; }
        public string Text { get; init; }
        public bool? IsMarkedAsRead { get; init; }
    }
}