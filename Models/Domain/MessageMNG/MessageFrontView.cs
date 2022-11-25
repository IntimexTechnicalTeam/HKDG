namespace Domain
{
    public class MessageFrontView
    {
        public Guid Id { get; set; }

        public string Subject { get; set; }

        public string Content { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
