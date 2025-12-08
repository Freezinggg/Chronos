namespace Chronos.Domain
{
    public class Event
    {
        public Guid Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventNotes { get; set; }
        public DateTime EventStartAt { get; set; }
        public DateTime EventEndAt { get; set; }
        public string Location { get; set; }
        public bool HasReminder { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public bool ValidateDate()
        {
            if (EventStartAt < DateTime.Now || EventEndAt < DateTime.Now) return false;

            return true;
        }
    }
}
