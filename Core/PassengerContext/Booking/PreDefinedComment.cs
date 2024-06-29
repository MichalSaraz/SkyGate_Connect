namespace Core.PassengerContext.Booking
{
    public class PredefinedComment
    {
        public string Id { get; private set; }
        public string Text { get; private set; }
        
        public PredefinedComment(string id, string text)
        {
            Id = id;
            Text = text;
        }
    }
}
