namespace Core.Dtos;

public class ActionHistoryDto
{
    public Guid Id { get; set; }
    public Guid PassengerOrItemId { get; set; }
    public string Type { get; set; }
    public string Action { get; set; }
    public List<object> SerializedOldValue { get; set; }
    public List<object> SerializedNewValue { get; set; }
    public DateTime Timestamp { get; set; }
}