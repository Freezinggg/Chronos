using Chronos.Application;
using Chronos.DTO.Event;
using Microsoft.AspNetCore.Mvc;

namespace Chronos.Controllers
{
    [Route("/Event")]
    public class EventController : Controller
    {
        EventServices _eventServices { get; set; }
        public EventController(EventServices eventServices)
        {
            _eventServices = eventServices;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventDto dto)
        {
            var result = _eventServices.CreateEvent(dto);
            return Json(result);
        }

        [HttpGet]
        public IActionResult GetEvents()
        {
            var result = _eventServices.GetEvents();
            return Json(result);
        }

        [HttpDelete]
        public IActionResult DeleteEvent(Guid id)
        {
            var result = _eventServices.DeleteEvent(id);
            return Json(result);
        }

        [HttpPut]
        public IActionResult UpdateEvent(Guid id, [FromBody] UpdateEventDto dto)
        {
            var result = _eventServices.DeleteEvent(id);
            return Json(result);
        }
    }
}
