namespace Chronos.DTO.Event
{
    public class EventDto
    {
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventNotes { get; set; }
        public DateTime EventStartAt { get; set; }
        public DateTime EventEndAt { get; set; }
        public string Location { get; set; }
        public bool HasReminder { get; set; }
    }
}
