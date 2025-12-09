using Chronos.Domain;

namespace Chronos.Application
{
    public interface IEventRepository
    {
        Event? Get(Guid id);
        Event? Create(Event e);
        IEnumerable<Event> GetEvent();
        bool Delete(Guid id);
        bool Update(Event e);
    }
}
