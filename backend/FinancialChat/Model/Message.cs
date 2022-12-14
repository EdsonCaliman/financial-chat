namespace FinancialChat.Model
{
    public class Message
    {
        public int Id { get; set; }
        public string Text { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Room { get; set; }
        public string User { get; set; }

        public Message()
        {
            CreatedAt = DateTime.UtcNow;
        }
    }
}