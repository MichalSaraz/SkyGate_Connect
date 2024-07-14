using Core.HistoryTracking.Enums;
using Core.PassengerContext;
using Newtonsoft.Json;

namespace Core.HistoryTracking;

public class ActionHistory<T> where T : class
{
    public Guid Id { get; set; }
    
    public BasePassengerOrItem PassengerOrItem { get; set; }
    public Guid PassengerOrItemId { get; set; }
    
    public ActionTypeEnum Action { get; set; }
    public DateTime Timestamp { get; set; }
    public string SerializedOldValue { get; set; }
    public string SerializedNewValue { get; set; }
    public string Type { get; set; }
    
    protected ActionHistory() { }

    public ActionHistory(ActionTypeEnum action, Guid passengerOrItemId, string typeName, T newValue = null,
        T oldValue = null)
    {
        Id = Guid.NewGuid();
        Action = action;
        PassengerOrItemId = passengerOrItemId;
        Type = typeName;
        SerializedOldValue = oldValue == null ? null : JsonConvert.SerializeObject(oldValue,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        SerializedNewValue = JsonConvert.SerializeObject(newValue,
            new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
        Timestamp = DateTime.UtcNow;
    }
}