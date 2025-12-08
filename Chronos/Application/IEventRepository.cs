using Chronos.Domain;

namespace Chronos.Application
{
    public interface IEventRepository
    {
        Event? Create(Event e);
        IEnumerable<Event> GetEvent();
    }
}
