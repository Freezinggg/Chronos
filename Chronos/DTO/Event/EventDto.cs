namespace Chronos.DTO.Event
{
    public class EventDto : EventBaseDto
    {
        public EventDto(Domain.Event x)
        {
            Id = x.Id;
            EventName = x.EventName;
            EventDescription = x.EventDescription;
            EventEndAt = x.EventEndAt;
            EventStartAt = x.EventStartAt;
            EventNotes = x.EventNotes;
            Location = x.Location;
        }
    }
}
