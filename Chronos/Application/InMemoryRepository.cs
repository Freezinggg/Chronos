using Chronos.Domain;

namespace Chronos.Application
{
    public class InMemoryRepository : IEventRepository
    {
        public static List<Event> Events = new List<Event>();
        public Event? Create(Event e)
        {
            try
            {
                e.Id = Guid.NewGuid();
                Events.Add(e);

                return e;
            }
            catch(Exception ex)
            {
                return null;
            }
            
        }

        public IEnumerable<Event> GetEvent()
        {
            return Events;
        }
    }
}
