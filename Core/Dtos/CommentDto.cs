namespace Core.Dtos
{
    public class CommentDto
    {
        public Guid Id { get; init; }
        public string CommentType { get; init; }
        public string Text { get; init; }
    }
}