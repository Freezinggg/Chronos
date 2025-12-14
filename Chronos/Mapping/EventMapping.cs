using Chronos.Domain;
using Chronos.DTO.Event;

namespace Chronos.Mapping
{
    public static class EventMapping
    {
        public static Event MappingToEntity(EventBaseDto dto)
        {
            Event e = new()
            {
                EventName = dto.EventName,
                EventStartAt = dto.EventStartAt,
                EventEndAt = dto.EventEndAt,
                EventDescription = dto.EventDescription,
                EventNotes = dto.EventNotes,
                Location = dto.Location,
                HasReminder = dto.HasReminder,
            };
            return e;
        }
    }
}
