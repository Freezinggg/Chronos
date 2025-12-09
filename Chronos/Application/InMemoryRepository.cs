using Chronos.Domain;

namespace Chronos.Application
{
    public class InMemoryRepository : IEventRepository
    {
        public static List<Event> Events = new List<Event>();

        public Event? Get(Guid id)
        {
            return Events.Where(x => x.Id == id).FirstOrDefault();
        }

        public Event? Create(Event e)
        {
            try
            {
                e.Id = Guid.NewGuid();
                Events.Add(e);

                return e;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        public IEnumerable<Event> GetEvent()
        {
            return Events;
        }

        public bool Delete(Guid id)
        {
            try
            {
                Event e = Events.Where(x => x.Id == id).FirstOrDefault();
                if (e == null) return false;

                e.DeletedAt = DateTime.Now;

                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool Update(Event e)
        {
            try
            {
                e.UpdatedAt = DateTime.Now;

                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
