using System.ComponentModel.DataAnnotations;

namespace Chronos.DTO.Event
{
    public class EventBaseDto
    {
        public EventBaseDto()
        {
            EventStartAt = DateTime.Now;
            EventEndAt = DateTime.Now;
        }

        public Guid? Id { get; set; }
        public string EventName { get; set; }
        public string EventDescription { get; set; }
        public string EventNotes { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EventStartAt { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime EventEndAt { get; set; }

        public string Location { get; set; }
        public bool HasReminder { get; set; }
    }
}
