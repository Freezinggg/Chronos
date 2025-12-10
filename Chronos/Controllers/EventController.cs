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
            return View("~/Views/Event/_CreateEventForm.cshtml");
        }

        [HttpPost("CreateEvent")]
        public IActionResult CreateEvent([FromBody] CreateEventDto dto)
        {
            var result = _eventServices.CreateEvent(dto);
            return Json(result);
        }

        [HttpPost("CheckOverlapEvent")]
        public IActionResult CheckOverlapEvent(Guid id, [FromBody] UpdateEventDto dto)
        {
            var result = _eventServices.CheckOverlapEvent(id, dto);
            return Json(result);
        }

        [HttpGet("GetEvents")]
        public IActionResult GetEvents(string? startDate, string? endDate)
        {
            var result = _eventServices.GetEvents(startDate, endDate);
            return Json(result);
        }

        [HttpDelete("DeleteEvent")]
        public IActionResult DeleteEvent(Guid id)
        {
            var result = _eventServices.DeleteEvent(id);
            return Json(result);
        }

        [HttpPut("UpdateEvent")]
        public IActionResult UpdateEvent(Guid id, [FromBody] UpdateEventDto dto)
        {
            var result = _eventServices.DeleteEvent(id);
            return Json(result);
        }
    }
}
