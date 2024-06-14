namespace Core.Dtos
{
    public class PassengerOrItemCommentsDto : BasePassengerOrItemDto
    {
        public List<CommentDto> Comments { get; set; } = new();
    }
}
